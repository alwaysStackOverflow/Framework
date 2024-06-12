using System;
using System.IO;
using System.Net;
using System.Buffers;
using System.Collections.Generic;
using Log = GameFramework.GameFrameworkLog;
using System.Collections.Concurrent;

namespace GameFramework.Network
{
	public class KcpService : IDisposable
	{
		public const long ConnectTimeoutTime = 20 * 1000;

		public const long ConnectRemainTime = 3 * 1000; //3秒没有收到消息就断开连接

		public Action<uint, IPEndPoint> AcceptCallback;
		public Action<uint, MemoryBuffer> ReadCallback;
		public Action<uint, int> ErrorCallback;
		private string _serviceTypeString;
		public ServiceType ServiceType { get; private set; }

		private readonly ConcurrentQueue<MemoryBuffer> _bufferPool = new();

		private readonly long _createTime;
		public long TimeNow => TimeInfo.ClientFrameStartTime - _createTime;

		public IKcpTransport Transport { get; private set; }

		private readonly Dictionary<long, KcpConnection> _connections = new();
		private readonly Dictionary<long, KcpConnection> _waitConnects = new();

		private readonly byte[] _cache = new byte[Utility.Math.OneK * 2];
		private EndPoint _endPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
		private readonly List<long> _cacheIds = new();
		private readonly HashSet<long> _updateIds = new();
		private readonly MultiMap<long, long> _timeID = new();
		private readonly List<long> _timeOutTime = new();
		public ArrayPool<byte> byteArrayPool = ArrayPool<byte>.Create(Utility.Math.OneM * 2, 200);

		private readonly Dictionary<uint, long> _connectionsConnectTime = new();
		private readonly Queue<uint> _connectionsConnectTimeOutId = new();

		public KcpService(ServiceType serviceType, ProtocolType protocolType, ushort prot)
		{
			switch (protocolType)
			{
				case ProtocolType.UdpV6:
				{
					Transport = new UdpTransport(new IPEndPoint(NetworkHelper.GetOsAddress(), prot));
					break;
				}
				case ProtocolType.UdpV4:
				{
					Transport = new UdpTransport(new IPEndPoint(NetworkHelper.GetOsAddress(false), prot));
					break;
				}
				default:
				{
					Transport = new UdpTransport(new IPEndPoint(NetworkHelper.GetOsAddress(), prot));
					break;
				}
			}
			_createTime = TimeInfo.ClientFrameStartTime;
			_serviceTypeString = $"{serviceType}_{protocolType}_{prot}";
			ServiceType = serviceType;
		}

		public override string ToString()
		{
			return _serviceTypeString;
		}

		public MemoryBuffer Fetch()
		{
			if (_bufferPool.TryDequeue(out var buffer))
			{
				return buffer;
			}
			return new MemoryBuffer(Utility.Math.OneK);
		}

		public void Recycle(MemoryBuffer memoryBuffer)
		{
			if (memoryBuffer.Capacity > 1024)
			{
				return;
			}

			if (_bufferPool.Count > Utility.Math.OneK)
			{
				return;
			}

			memoryBuffer.Seek(0, SeekOrigin.Begin);
			memoryBuffer.SetLength(0);

			_bufferPool.Enqueue(memoryBuffer);
		}

		public bool IsDisposed()
		{
			return Transport == null;
		}

		public void Dispose()
		{
			if (IsDisposed())
			{
				return;
			}
			foreach (var id in _connections.Keys)
			{
				Remove(id);
			}
			Transport.Dispose();
			Transport = null;
		}

		public (uint, uint) GetConnection(long id)
		{
			var connection = Get(id);
			return connection == null ? throw new Exception($"GetConnection conn not found Kcp! Id:{id}") : (connection.LocalConnectionId, connection.RemoteConnectionId);
		}

		public void ChangeAddress(long id, IPEndPoint ipEndPoint)
		{
			var connection = Get(id);
			if (connection == null)
			{
				return;
			}
			connection.RemoteIPEndPoint = ipEndPoint;
		}

		private void Receive()
		{
			if (Transport == null)
			{
				Log.Warning("Receive Transport is null");
				return;
			}
			while (Transport != null && Transport.Available() > 0)
			{
				int messageLength = Transport.Receive(_cache, ref _endPoint);

				if (messageLength < 1)
				{
					continue;
				}
				var kcpOptionType = _cache[0];
				uint localConn = 0;
				uint remoteConn = 0;
				try
				{
					KcpConnection connection = null;
					switch (kcpOptionType)
					{
						case NetworkMessageType.ClientConnect:
						{
							if (messageLength != 9)
							{
								break;
							}
							remoteConn = BitConverter.ToUInt32(_cache, 1);
							localConn = BitConverter.ToUInt32(_cache, 5);
							_waitConnects.TryGetValue(remoteConn, out connection);
							if (connection == null)
							{
								localConn = KcpConnection.CreateAcceptChannelId();
								while (_connections.ContainsKey(localConn))
								{
									localConn = KcpConnection.CreateAcceptChannelId();
								}
								connection = new KcpConnection(localConn, remoteConn, _endPoint.Clone(), this);
								_waitConnects.Add(connection.RemoteConnectionId, connection);
								_connections.Add(connection.LocalConnectionId, connection);
							}

							if (connection.RemoteConnectionId != remoteConn)
							{
								break;
							}

							try
							{
								_cache.WriteTo(0, NetworkMessageType.ServerAccept);
								_cache.WriteTo(1, connection.LocalConnectionId);
								_cache.WriteTo(5, connection.RemoteConnectionId);
								Transport.Send(_cache, 0, 9, connection.RemoteIPEndPoint);
								AcceptCallback?.Invoke(connection.LocalConnectionId, connection.RemoteIPEndPoint);
								Log.Info($"{ToString()} Send ServerAccept: LocalConnectionId:{connection.LocalConnectionId} RemoteConnectionId:{remoteConn} RemoteIPEndPoint:{connection.RemoteIPEndPoint}");
								if (!_connectionsConnectTime.ContainsKey(localConn))
								{
									_connectionsConnectTime.Add(localConn, TimeInfo.LocalTimeTicks);
								}
								else
								{
									_connectionsConnectTime[localConn] = TimeInfo.LocalTimeTicks;
								}
							}
							catch (Exception e)
							{
								Log.Error(e);
								connection.OnError(ErrorCode.ERR_SocketCantSend);
							}
							break;
						}
						case NetworkMessageType.ServerAccept:
						{
							if (messageLength != 9)
							{
								break;
							}
							remoteConn = BitConverter.ToUInt32(_cache, 1);
							localConn = BitConverter.ToUInt32(_cache, 5);
							connection = Get(localConn);
							if (connection != null)
							{
								Log.Info($"{ToString()} Receive ServerAccept: LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn}");
								connection.RemoteConnectionId = remoteConn;
								connection.HandleConnect();
								AcceptCallback?.Invoke(connection.LocalConnectionId, connection.RemoteIPEndPoint);
							}
							break;
						}
						case NetworkMessageType.Disconnect: // 断开
						{
							if (messageLength != 13)
							{
								break;
							}
							remoteConn = BitConverter.ToUInt32(_cache, 1);
							localConn = BitConverter.ToUInt32(_cache, 5);
							int error = BitConverter.ToInt32(_cache, 9);
							connection = Get(localConn);
							if (connection == null)
							{
								break;
							}
							if (connection.RemoteConnectionId != remoteConn)
							{
								break;
							}
							_connectionsConnectTime.Remove(localConn);
							Log.Info($"{ToString()} Receive Disconnect: LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn} Code:{error}");
							connection.OnError(ErrorCode.ERR_PeerDisconnect);
							break;
						}
						case NetworkMessageType.GameMessage:
						{
							if (messageLength < 9)
							{
								break;
							}
							remoteConn = BitConverter.ToUInt32(_cache, 1);
							localConn = BitConverter.ToUInt32(_cache, 5);
							connection = Get(localConn);
							if (connection == null)
							{
								Disconnect(localConn, remoteConn, ErrorCode.ERR_KcpNotFoundChannel, _endPoint, 1);
								break;
							}
							if (connection.RemoteConnectionId != remoteConn)
							{
								break;
							}
							if (!connection.IsConnected)
							{
								connection.IsConnected = true;
								_waitConnects.Remove(connection.RemoteConnectionId);
							}
							connection.HandleReceive(_cache, 9, messageLength - 9);
							//Log.Info($"{ToString()} Receive GameMessage: LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn}, Length: {messageLength - 9}");
							break;
						}
						case NetworkMessageType.PingRequest:
						{
							if (messageLength != 17)
							{
								break;
							}

							remoteConn = BitConverter.ToUInt32(_cache, 1);
							localConn = BitConverter.ToUInt32(_cache, 5);
							var clientTime = BitConverter.ToInt64(_cache, 9);
							connection = Get(localConn);
							//Log.Info($"{ServiceType} Receive PingRequest: LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn}");
							if (connection == null)
							{
								Disconnect(localConn, remoteConn, ErrorCode.ERR_KcpNotFoundChannel, _endPoint, 1);
								break;
							}
							if (connection.RemoteConnectionId != remoteConn)
							{
								break;
							}
							if (!connection.IsConnected)
							{
								connection.IsConnected = true;
								_waitConnects.Remove(connection.RemoteConnectionId);
							}
							if (!_connectionsConnectTime.ContainsKey(localConn))
							{
								_connectionsConnectTime.Add(localConn, TimeInfo.LocalTimeTicks);
							}
							else
							{
								_connectionsConnectTime[localConn] = TimeInfo.LocalTimeTicks;
							}
							try
							{
								_cache.WriteTo(0, NetworkMessageType.PingReply);
								_cache.WriteTo(1, connection.LocalConnectionId);
								_cache.WriteTo(5, connection.RemoteConnectionId);
								_cache.WriteTo(9, clientTime);
								_cache.WriteTo(17, TimeInfo.LocalTimeTicks);
								Transport.Send(_cache, 0, 25, connection.RemoteIPEndPoint);
								//Log.Info($"{ServiceType} Send PingReply: LocalConnectionId:{connection.LocalConnectionId} RemoteConnectionId:{remoteConn} RemoteIPEndPoint:{connection.RemoteIPEndPoint}");
							}
							catch (Exception e)
							{
								Log.Error(e);
								connection.OnError(ErrorCode.ERR_SocketCantSend);
							}
							break;
						}
						case NetworkMessageType.PingReply:
						{
							if (messageLength != 25)
							{
								break;
							}

							remoteConn = BitConverter.ToUInt32(_cache, 1);
							localConn = BitConverter.ToUInt32(_cache, 5);
							var lastSendTime = BitConverter.ToInt64(_cache, 9);
							var serverTime = BitConverter.ToInt64(_cache, 17);
							connection = Get(localConn);
							//Log.Info($"{ServiceType} Receive PingReply: LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn}");
							if (connection == null)
							{
								Disconnect(localConn, remoteConn, ErrorCode.ERR_KcpNotFoundChannel, _endPoint, 1);
								break;
							}
							if (connection.RemoteConnectionId != remoteConn)
							{
								break;
							}
							var clientNow = TimeInfo.ClientTime;
							TimeInfo.Delay = (clientNow - lastSendTime) / 2;
							TimeInfo.ServerTimeOffset = serverTime + TimeInfo.Delay - clientNow;
							//Log.Info($"Ping:{TimeInfo.Delay}: LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn}");
							break;
						}

					}
				}
				catch (Exception e)
				{
					Log.Error($"{ToString()} Receive Message Error: NetworkMessageType:{kcpOptionType} LocalConnectionId:{localConn} RemoteConnectionId:{remoteConn} \n{e.Message}\n{e.StackTrace}");
				}
			}
		}

		public KcpConnection Get(long id)
		{
			_connections.TryGetValue(id, out var connection);
			return connection;
		}

		public KcpConnection Create(IPEndPoint ipEndPoint)
		{
			var id = KcpConnection.CreateAcceptChannelId();
			while (_connections.ContainsKey(id))
			{
				id = KcpConnection.CreateAcceptChannelId();
			}
			try
			{
				var connection = new KcpConnection(id, ipEndPoint, this);
				_connections.Add(id, connection);
				AddToUpdate(0, id);
				return connection;
			}
			catch (Exception e)
			{
				Log.Error($"Create KcpService Error Type:{ToString()}\n{e}");
			}
			return null;
		}

		public void Remove(long id, int code = 0)
		{
			var connection = Get(id);
			if (connection == null)
			{
				return;
			}
			connection.Code = code;
			Log.Info($"{ToString()} Remove channel: LocalConnectionId:{connection.LocalConnectionId} RemoteConnectionId:{connection.RemoteConnectionId} Code:{code}");
			if (_waitConnects.TryGetValue(connection.RemoteConnectionId, out var waitConnection))
			{
				if (waitConnection.LocalConnectionId == connection.LocalConnectionId)
				{
					_waitConnects.Remove(connection.RemoteConnectionId);
				}
			}
			connection.Dispose();
			Transport.OnError(id, code);
		}

		public void Disconnect(uint localConn, uint remoteConn, int error, EndPoint address, int times)
		{
			try
			{
				if (Transport == null)
				{
					return;
				}
				_cache.WriteTo(0, NetworkMessageType.Disconnect);
				_cache.WriteTo(1, (int)localConn);
				_cache.WriteTo(5, (int)remoteConn);
				_cache.WriteTo(9, (uint)error);
				for (int i = 0; i < times; i++)
				{
					Transport.Send(_cache, 0, 13, address);
				}
			}
			catch (Exception e)
			{
				Log.Error($"Disconnect error {localConn} {remoteConn} {error} {address} {e}");
			}
			Log.Info($"{ToString()} Send Disconnect: localConn:{localConn} remoteConn:{remoteConn} Ip:{address} Code:{error}");
		}

		public void SendHeartbeat(uint localConn)
		{
			try
			{
				if (Transport == null)
				{
					return;
				}
				var connection = Get(localConn);
				if (connection == null)
				{
					return;
				}
				_cache.WriteTo(0, NetworkMessageType.PingRequest);
				_cache.WriteTo(1, (int)connection.LocalConnectionId);
				_cache.WriteTo(5, (int)connection.RemoteConnectionId);
				_cache.WriteTo(9, TimeInfo.ClientTime);
				Transport.Send(_cache, 0, 17, connection.RemoteIPEndPoint);
				//Log.Info($"Send PingRequest: {connection.LocalConnectionId} {connection.RemoteConnectionId}");
			}
			catch (Exception e)
			{
				Log.Error($"Send PingRequest Error LocalConnectionId:{localConn}\n{e.Message}\n{e.StackTrace}");
			}
		}

		public void Send(long channelId, MemoryBuffer buffer)
		{
			Get(channelId)?.Send(buffer);
		}

		public void Update(float elapseSeconds = 0, float realElapseSeconds = 0)
		{
			var now = TimeNow;
			CheckConnectTimeOut(now);
			CheckWaitAcceptChannel(now);
			Receive();
			UpdateChannel(now);
			Transport.Update();
			CheckConnectionsIsConnecting();
		}

		private void CheckWaitAcceptChannel(long timeNow)
		{
			_cacheIds.Clear();
			foreach (var connection in _waitConnects.Values)
			{
				if (connection.IsDisposed)
				{
					continue;
				}

				if (connection.IsConnected)
				{
					continue;
				}

				if (timeNow - connection.CreateTime < ConnectTimeoutTime)
				{
					continue;
				}
				_cacheIds.Add(connection.LocalConnectionId);
			}

			foreach (long id in _cacheIds)
			{
				if (!_waitConnects.TryGetValue(id, out var connection))
				{
					continue;
				}
				connection.OnError(ErrorCode.ERR_KcpAcceptTimeout);
			}
		}

		private void UpdateChannel(long timeNow)
		{
			foreach (var id in _updateIds)
			{
				var connection = Get(id);
				if (connection != null && id != 0)
				{
					connection.Update(timeNow);
				}
			}
			_updateIds.Clear();
		}

		public void AddToUpdate(long time, long id)
		{
			if (time == 0)
			{
				_updateIds.Add(id);
				return;
			}
			_timeID.Add(TimeNow + time, id);
		}

		private void CheckConnectTimeOut(long timeNow)
		{
			if (_timeID.Count == 0)
			{
				return;
			}
			_timeOutTime.Clear();

			foreach (var time in _timeID.Keys)
			{
				if (time <= timeNow)
				{
					_timeOutTime.Add(time);
					break;
				}
			}

			foreach (var time in _timeOutTime)
			{
				if (_timeID.TryGetValue(time, out var idList))
				{
					foreach (var id in idList)
					{
						_updateIds.Add(id);
					}
				}
				_timeID.Remove(time);
			}
		}

		/// <summary>
		/// 给服务器用的，一段时间没有收到心跳包，认为客户端已经断开
		/// </summary>
		private void CheckConnectionsIsConnecting()
		{
			if (_connectionsConnectTime.Count == 0)
			{
				return;
			}
			_connectionsConnectTimeOutId.Clear();
			long now = TimeInfo.ServerFrameStartTime;
			foreach (var kv in _connectionsConnectTime)
			{
				var id = kv.Key;
				var time = kv.Value;
				if (now - time > ConnectRemainTime)
				{
					var connection = Get(id);
					if (connection != null)
					{
						Disconnect(connection.LocalConnectionId, connection.RemoteConnectionId, ErrorCode.ERR_PeerDisconnect, connection.RemoteIPEndPoint, 3);
					}
					_connectionsConnectTimeOutId.Enqueue(id);
				}
			}
			while (_connectionsConnectTimeOutId.Count > 0)
			{
				_connectionsConnectTime.Remove(_connectionsConnectTimeOutId.Dequeue());
			}
		}
	}
}
