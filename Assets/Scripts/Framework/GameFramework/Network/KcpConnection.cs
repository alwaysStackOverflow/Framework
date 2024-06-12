using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Concurrent;
using Log = GameFramework.GameFrameworkLog;

namespace GameFramework.Network
{
	public class KcpConnection : AConnection
	{
		private static long _connectionId = 0;
		public static uint CreateAcceptChannelId()
		{
			return (uint)Interlocked.Add(ref _connectionId, 1);
		}

		private const int MaxKcpMessageSize = 10000;
		private readonly KcpService _service;
		private Kcp _kcp;
		private readonly ConcurrentQueue<MemoryBuffer> _messageQueue = new();
		public readonly long CreateTime;

		public bool IsConnected { get; set; }

		private MemoryBuffer _readBuffer;
		private int _needReadSplitCount;
		private long _lastConnectTime = 0;

		private readonly byte[] _sendCache = new byte[Utility.Math.OneM * 2];

		private void InitKcp()
		{
			_kcp.SetNoDelay(1, 100, 2, true);
			_kcp.SetWindowSize(512, 512);
			_kcp.SetMtu(1400);
			_kcp.SetMinrto(200);
			_kcp.SetArrayPool(_service.byteArrayPool);
		}

		public KcpConnection(uint id, IPEndPoint remoteAddress, KcpService service)
		{
			_service = service;
			LocalConnectionId = id;
			ConnectionType = ConnectionType.Connect;
			RemoteIPEndPoint = remoteAddress;
			CreateTime = service.TimeNow;
			Log.Info($"Channel create: LocalConnectionId:{id} RemoteIP:{remoteAddress} ConnectionType:{ConnectionType}");
		}

		public KcpConnection(uint id, uint remoteId, IPEndPoint remoteAddress, KcpService service)
		{
			_service = service;
			ConnectionType = ConnectionType.Accept;
			LocalConnectionId = id;
			RemoteIPEndPoint = remoteAddress;
			RemoteConnectionId = remoteId;
			_kcp = new Kcp(remoteId, Output);
			InitKcp();
			CreateTime = service.TimeNow;
			Log.Info($"channel create: LocalConnectionId:{id} RemoteId:{remoteId} RemoteIP:{remoteAddress} ConnectionType:{ConnectionType}");
		}

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}
			Log.Info($"Channel dispose: LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId} Code:{Code}");
			try
			{
				if (Code != ErrorCode.ERR_PeerDisconnect)
				{
					_service.Disconnect(LocalConnectionId, RemoteConnectionId, Code, RemoteIPEndPoint, 3);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}
			LocalConnectionId = 0;
			RemoteConnectionId = 0;
			_kcp = null;
		}

		public void HandleConnect()
		{
			if (IsConnected)
			{
				return;
			}
			_kcp = new Kcp(LocalConnectionId, Output);
			InitKcp();
			Log.Info($"HandleConnect: LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId} IP:{RemoteIPEndPoint}");
			IsConnected = true;

			while (_messageQueue.Count > 0)
			{
				if (_messageQueue.TryDequeue(out var memoryBuffer))
				{
					Send(memoryBuffer);
				}
			}
		}

		private void Connect(long timeNow)
		{
			try
			{
				if (_lastConnectTime == 0)
				{
					_sendCache.WriteTo(0, NetworkMessageType.ClientConnect);
					_sendCache.WriteTo(1, LocalConnectionId);
					_sendCache.WriteTo(5, RemoteConnectionId);
					_service.Transport.Send(_sendCache, 0, 9, RemoteIPEndPoint);
					Log.Info($"Connect LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId} RemoteIP:{RemoteIPAddress}");
					_lastConnectTime = timeNow;
				}

				if (timeNow - _lastConnectTime > KcpService.ConnectTimeoutTime)
				{
					_lastConnectTime = 0;
					Log.Error($"Connect timeout: LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId} timeNow:{timeNow} CreateTime:{CreateTime} ConnectionType:{ConnectionType} RemoteIP:{RemoteIPEndPoint}");
					OnError(ErrorCode.ERR_KcpConnectTimeout);
					return;
				}
				else
				{
					_service.AddToUpdate(100, LocalConnectionId);
				}
				
			}
			catch (Exception e)
			{
				_lastConnectTime = 0;
				Log.Error(e);
				OnError(ErrorCode.ERR_SocketCantSend);
			}
		}

		public void Update(long timeNow)
		{
			if (IsDisposed)
			{
				return;
			}
			// 如果还没连接上，发送连接请求
			if (!IsConnected && ConnectionType == ConnectionType.Connect)
			{
				Connect(timeNow);
				return;
			}
			if (_kcp == null)
			{
				return;
			}
			try
			{
				_kcp.Update((uint)timeNow);
			}
			catch (Exception e)
			{
				Log.Error(e);
				OnError(ErrorCode.ERR_SocketError);
			}
			_service.AddToUpdate(_kcp.Check((uint)timeNow), LocalConnectionId);
		}

		public void HandleReceive(byte[] data, int offset, int length)
		{
			if (IsDisposed)
			{
				return;
			}

			_kcp?.Input(data.AsSpan(offset, length));
			_service.AddToUpdate(0, LocalConnectionId);
			while (true)
			{
				if (IsDisposed)
				{
					break;
				}

				var kcpPeekSize = _kcp.PeekSize();

				if (kcpPeekSize < 0)
				{
					break;
				}

				if (kcpPeekSize == 0)
				{
					OnError((int)SocketError.NetworkReset);
					return;
				}

				if (_needReadSplitCount == 0) // 消息没有分片
				{
					_readBuffer = _service.Fetch();
					_readBuffer.SetLength(kcpPeekSize);
					_readBuffer.Seek(0, SeekOrigin.Begin);
					int count = _kcp.Receive(_readBuffer.GetBuffer().AsSpan(0, kcpPeekSize));
					if (kcpPeekSize != count)
					{
						_service.Recycle(_readBuffer);
						_readBuffer = null;
						break;
					}
					if (kcpPeekSize == 8)// 判断是不是分片
					{
						if (BitConverter.ToInt32(_readBuffer.GetBuffer(), 0) == 0)
						{
							_needReadSplitCount = BitConverter.ToInt32(_readBuffer.GetBuffer(), 4);
							if (_needReadSplitCount <= MaxKcpMessageSize)
							{
								Log.Error($"Kcp read error3: _needReadSplitCount:{_needReadSplitCount} LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId}");
								OnError(ErrorCode.ERR_KcpSplitCountError);
								_service.Recycle(_readBuffer);
								_readBuffer = null;
								return;
							}
							_readBuffer.SetLength(_needReadSplitCount);
							_readBuffer.Seek(0, SeekOrigin.Begin);
							continue;
						}
					}
				}
				else
				{
					int count = _kcp.Receive(_readBuffer.GetBuffer().AsSpan((int)(_readBuffer.Length - _needReadSplitCount), kcpPeekSize));
					_needReadSplitCount -= count;
					if (kcpPeekSize != count)
					{
						Log.Error($"Kcp read error1: LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId}");
						OnError(ErrorCode.ERR_KcpReadNotSame);
						_service.Recycle(_readBuffer);
						_readBuffer = null;
						return;
					}
					if (_needReadSplitCount < 0)
					{
						Log.Error($"Kcp read error2: LocalConnectionId:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId}");
						OnError(ErrorCode.ERR_KcpSplitError);
						_service.Recycle(_readBuffer);
						_readBuffer = null;
						return;
					}
					if (_needReadSplitCount != 0)
					{
						continue;
					}
				}

				if(_readBuffer != null)
				{
					_readBuffer.Seek(0, SeekOrigin.Begin);
					OnRead(_readBuffer);
					_service.Recycle(_readBuffer);
					_readBuffer = null;
				}
			}
		}

		private void Output(byte[] buffer, int size)
		{
			if (IsDisposed || !IsConnected)
			{
				return;
			}

			try
			{
				if (size == 0)
				{
					Log.Error("Kcp send size is 0");
					return;
				}
				buffer.WriteTo(0, NetworkMessageType.GameMessage);
				buffer.WriteTo(1, LocalConnectionId);
				buffer.WriteTo(5, RemoteConnectionId);
				_service.Transport.Send(buffer, 0, size + 9, RemoteIPEndPoint);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void KcpSend(MemoryBuffer buffer)
		{
			if (IsDisposed)
			{
				return;
			}
			var count = (int)buffer.Position;
			if (buffer.Position <= MaxKcpMessageSize)
			{
				_kcp.Send(buffer.GetBuffer().AsSpan(0, count));
			}
			else// 超出maxPacketSize需要分片
			{
				// 先发分片信息
				_sendCache.WriteTo(0, 0);
				_sendCache.WriteTo(4, count);
				_kcp.Send(_sendCache.AsSpan(0, 8));

				int alreadySendCount = 0;
				while (alreadySendCount < count)
				{
					int leftCount = count - alreadySendCount;

					int sendCount = leftCount < MaxKcpMessageSize ? leftCount : MaxKcpMessageSize;

					_kcp.Send(buffer.GetBuffer().AsSpan(alreadySendCount, sendCount));

					alreadySendCount += sendCount;
				}
			}
			_service.AddToUpdate(0, LocalConnectionId);
		}

		public void Send(MemoryBuffer buffer)
		{
			if (!IsConnected)
			{
				_messageQueue.Enqueue(buffer);
				return;
			}
			if (_kcp == null)
			{
				throw new Exception("Kcp connected but _kcp is Null!");
			}
			var waitSendCount = _kcp.WaitSnd;
			if (waitSendCount > Utility.Math.OneM)
			{
				Log.Error($"Kcp wait snd too large: waitSendCount:{waitSendCount}: ID:{LocalConnectionId} RemoteConnectionId:{RemoteConnectionId}");
				OnError(ErrorCode.ERR_KcpWaitSendSizeTooLarge);
				return;
			}
			KcpSend(buffer);
			_service.Recycle(buffer);
		}


		private void OnRead(MemoryBuffer buffer)
		{
			try
			{
				_service.ReadCallback(LocalConnectionId, buffer);
			}
			catch (Exception e)
			{
				Log.Error($"{e}\n{e.StackTrace}");
				OnError(ErrorCode.ERR_PacketParserError);
			}
		}

		public void OnError(int error)
		{
			long channelId = LocalConnectionId;
			_service.Remove(LocalConnectionId, error);
			_service.ErrorCallback(LocalConnectionId, error);
		}
	}
}

