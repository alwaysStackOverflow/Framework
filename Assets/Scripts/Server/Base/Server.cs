using Common;
using GameFramework;
using GameFramework.Network;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using UnityGameFramework;
namespace Server
{
	internal class Server
	{
		internal readonly string MainCountConnenctionToken;
		internal string DatabasePath { get; private set; }

		private readonly KcpService _udpV6Service;
		private readonly KcpService _udpV4Service;
		private readonly ConcurrentDictionary<long, NetworkHandler> _handlerList;

		internal DatabaseData Database { get; private set; }
		internal CacheData CacheData { get; private set; }

		public Server(string database)
		{
			DatabasePath = database;
			MainCountConnenctionToken = Guid.NewGuid().ToString();
			Database = new DatabaseData(DatabasePath);
			CacheData = new CacheData(Database);
			_handlerList = new();

			_udpV6Service = new(ServiceType.Server, ProtocolType.UdpV6, PortType.ServerUdpV6);
			_udpV6Service.AcceptCallback += OnAcceptCallbackUdpV6;
			_udpV6Service.ReadCallback += OnReadCallbackUdpV6;
			_udpV6Service.ErrorCallback += OnErrorCallbackUdpV6;

			_udpV4Service = new(ServiceType.Server, ProtocolType.UdpV4, PortType.ServerUdpV4);
			_udpV4Service.AcceptCallback += OnAcceptCallbackUdpV4;
			_udpV4Service.ReadCallback += OnReadCallbackUdpV4;
			_udpV4Service.ErrorCallback += OnErrorCallbackUdpV4;
		}

		internal void Init()
		{
			CacheData.Init();
		}

		public void Update()
		{
			_udpV6Service?.Update();
			_udpV4Service?.Update();
		}

		public void Shutdown()
		{
			foreach (var handler in _handlerList.Values)
			{
				ReferencePool.Release(handler);
			}
			_handlerList.Clear();
			_udpV6Service?.Dispose();
			_udpV4Service?.Dispose();
			Database?.Dispose();
			CacheData?.Dispose();
		}

		private const long UdpV6 = ((long)ProtocolType.UdpV6) << 32;
		private const long UdpV4 = ((long)ProtocolType.UdpV4) << 32;

		public void Send(long type, uint connectionId, ProtoObject data)
		{
			switch (type)
			{
				case UdpV6:
				{
					var connection = _udpV6Service.Get(connectionId);
					if (connection != null)
					{
						var memoryBuffer = _udpV6Service.Fetch();
						NetworkHelper.MessageToStream(memoryBuffer, data);
						_udpV6Service.Send(connectionId, memoryBuffer);
						Log.Info($"Server UdpV6 Send <color=#ffff00>ConnectionId:{connectionId}</color> <color=#00ffff>ID:{data.ProtocolID}</color>\nMessage:{data}");
					}

					break;
				}
				case UdpV4:
				{
					var connection = _udpV4Service.Get(connectionId);
					if (connection != null)
					{
						var memoryBuffer = _udpV4Service.Fetch();
						NetworkHelper.MessageToStream(memoryBuffer, data);
						_udpV4Service.Send(connectionId, memoryBuffer);
						Log.Info($"Server UdpV4 Send <color=#ffff00>ConnectionId:{connectionId}</color> <color=#00ffff>ID:{data.ProtocolID}</color>\nMessage:{data}");
					}
					break;
				}
			}
			ReferencePool.Release(data);
		}

		private void OnAcceptCallbackUdpV6(uint connectionId, IPEndPoint endpoint)
		{
			var handler = NetworkHandler.Create(UdpV6, connectionId, this, CacheData);
			_handlerList.TryAdd(handler.Id, handler);
			Log.Info($"<color=#88ff00>Server</color>  UdpV6 OnAcceptCallbackUdpV6 ConnectionId:{connectionId} IP:{endpoint}");
		}

		private void OnReadCallbackUdpV6(uint connectionId, MemoryBuffer steam)
		{
			var buffer = steam.GetBuffer();
			var protocolID = BitConverter.ToInt32(buffer, 0);
			if (ProtocolTypeReference.TryGet(protocolID, out Type type))
			{
				var message = NetworkHelper.Deserialize(type, buffer.AsSpan(4, buffer.Length - 4));
				Log.Info($"<color=#88ff00>Server</color> UdpV6 Receive <color=#00ff00>ID:{protocolID} Code:{message.Code}</color>\nMessage:{message}");
				ServerEntry.NetworkEvent.TriggerNow(UdpV6 + connectionId, message);
			}
		}

		private void OnErrorCallbackUdpV6(uint connectionId, int code)
		{
			Log.Error($"<color=#88ff00>Server</color>  Error LocalConnectionId:{connectionId} Code {code}");
			if (_handlerList.TryRemove(UdpV6 + connectionId, out var handler))
			{
				ReferencePool.Release(handler);
			}
		}

		private void OnAcceptCallbackUdpV4(uint connectionId, IPEndPoint endpoint)
		{
			var handler = NetworkHandler.Create(UdpV4, connectionId, this, CacheData);
			_handlerList.TryAdd(handler.Id, handler);
			Log.Info($"<color=#88ff00>Server</color>  UdpV4 OnAcceptCallbackUdpV6 ConnectionId:{connectionId} IP:{endpoint}");
		}

		private void OnReadCallbackUdpV4(uint connectionId, MemoryBuffer steam)
		{
			var buffer = steam.GetBuffer();
			var protocolID = BitConverter.ToInt32(buffer, 0);
			if (ProtocolTypeReference.TryGet(protocolID, out Type type))
			{
				var message = NetworkHelper.Deserialize(type, buffer.AsSpan(4, buffer.Length - 4));
				Log.Info($"<color=#88ff00>Server</color>  UdpV4 Receive <color=#ffff00>ID:{protocolID} Code:{message.Code}</color>\nMessage:{message}");
				ServerEntry.NetworkEvent.TriggerNow(UdpV4 + connectionId, message);
			}
		}

		private void OnErrorCallbackUdpV4(uint connectionId, int code)
		{
			Log.Error($"<color=#88ff00>Server</color>  Error LocalConnectionId:{connectionId} Code {code}");
			if (_handlerList.TryRemove(UdpV4 + connectionId, out var handler))
			{
				ReferencePool.Release(handler);
			}
		}
	}
}