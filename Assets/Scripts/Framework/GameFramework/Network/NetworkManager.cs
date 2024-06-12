using System;
using System.Net;
using Log = GameFramework.GameFrameworkLog;

namespace GameFramework.Network
{
	internal class NetworkManager : GameFrameworkModule, INetworkManager
	{
		internal override int Priority => NetworkManagerPriority;

		private KcpService _networkServiceV6;
		private KcpService _networkServiceV4;
		private KcpConnection _currentConnection;
		private ProtocolType _currentServiceType;

		private Action _acceptCallback;
		private Action<MemoryBuffer> _readCallback;
		private Action<int> _errorCallback;

		public event Action AcceptCallback
		{
			add { _acceptCallback += value; }
			remove { _acceptCallback -= value; }
		}

		public event Action<MemoryBuffer> ReadCallback
		{
			add { _readCallback += value; }
			remove { _readCallback -= value; }
		}

		public event Action<int> ErrorCallback
		{
			add { _errorCallback += value; }
			remove { _errorCallback -= value; }
		}

		public NetworkManager()
		{
			_networkServiceV6 = new KcpService(ServiceType.Client, ProtocolType.UdpV6, PortType.ClientUdpV6);
			_networkServiceV6.AcceptCallback += OnAcceptCallback;
			_networkServiceV6.ReadCallback += OnReadCallback;
			_networkServiceV6.ErrorCallback += OnErrorCallback;

			_networkServiceV4 = new KcpService(ServiceType.Client, ProtocolType.UdpV4, PortType.ClientUdpV4);
			_networkServiceV4.AcceptCallback += OnAcceptCallback;
			_networkServiceV4.ReadCallback += OnReadCallback;
			_networkServiceV4.ErrorCallback += OnErrorCallback;

			_acceptCallback = null;
			_readCallback = null;
			_errorCallback = null;
		}


		internal override void Shutdown()
		{
			_networkServiceV6.Dispose();
			_networkServiceV6 = null;
			_networkServiceV4.Dispose();
			_networkServiceV4 = null;
		}

		internal override void Update(float elapseSeconds, float realElapseSeconds)
		{
			if (_currentServiceType == ProtocolType.UdpV6)
			{
				_networkServiceV6.Update(elapseSeconds, realElapseSeconds);
			}
			else
			{
				_networkServiceV4.Update(elapseSeconds, realElapseSeconds);
			}
		}

		public void Connect(IPEndPoint ipEndPointV6, IPEndPoint ipEndPointV4)
		{
			Log.Info("Start Connect");
			//服务器和客户端都支持ipv6的话，就使用ipv6
			if (ipEndPointV6 != default && NetworkHelper.GetOsServiceType() == ProtocolType.UdpV6)
			{
				Log.Info($"Connect UdpV6 {ipEndPointV6}");
				_currentConnection = _networkServiceV6.Create(ipEndPointV6);
			}
			else
			{
				Log.Info($"Connect UdpV4 {ipEndPointV4}");
				_currentConnection = _networkServiceV4.Create(ipEndPointV4);
			}
			_currentServiceType = NetworkHelper.GetOsServiceType();
		}

		public void SendHeartbeat()
		{
			if(_currentServiceType == ProtocolType.UdpV6)
			{
				_networkServiceV6.SendHeartbeat(_currentConnection.LocalConnectionId);
			}
			else
			{
				_networkServiceV4.SendHeartbeat(_currentConnection.LocalConnectionId);
			}
		}

		public void Disconnect()
		{
			if (_currentConnection == null)
			{
				return;
			}
			if (_currentServiceType == ProtocolType.UdpV6)
			{
				_networkServiceV6.Disconnect(_currentConnection.LocalConnectionId, _currentConnection.RemoteConnectionId, ErrorCode.ERR_PeerDisconnect, _currentConnection.RemoteIPEndPoint, 1);

			}
			else
			{
				_networkServiceV4.Disconnect(_currentConnection.LocalConnectionId, _currentConnection.RemoteConnectionId, ErrorCode.ERR_PeerDisconnect, _currentConnection.RemoteIPEndPoint, 1);
			}
		}

		public void Send(ProtoObject data)
		{
			if (_currentServiceType == ProtocolType.UdpV6)
			{
				var memoryBuffer = _networkServiceV6.Fetch();
				NetworkHelper.MessageToStream(memoryBuffer, data);
				_networkServiceV6.Send(_currentConnection.LocalConnectionId, memoryBuffer);
			}
			else
			{
				var memoryBuffer = _networkServiceV4.Fetch();
				NetworkHelper.MessageToStream(memoryBuffer, data);
				_networkServiceV4.Send(_currentConnection.LocalConnectionId, memoryBuffer);
			}
			Log.Info($"<color=#ff8800>Server</color>  Send <color=#00ffff>ID:{data.ProtocolID}</color> \n Message:{data}");
			ReferencePool.Release(data);
		}

		private void OnAcceptCallback(uint connectionId, IPEndPoint ipEndPoint)
		{
			if(_currentConnection != null && connectionId == _currentConnection.LocalConnectionId)
			{
				_acceptCallback?.Invoke();
			}
		}

		private void OnReadCallback(uint connectionId, MemoryBuffer buffer)
		{
			if (_currentConnection != null && connectionId == _currentConnection.LocalConnectionId)
			{
				_readCallback?.Invoke(buffer);
			}
			
		}

		private void OnErrorCallback(uint connectionId, int code)
		{
			if (_currentConnection != null && connectionId == _currentConnection.LocalConnectionId)
			{
				_errorCallback?.Invoke(code);
			}
			
		}
	}
}
