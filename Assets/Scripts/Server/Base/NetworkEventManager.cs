using System.Threading;
using GameFramework.Network;
using System.Collections.Concurrent;
using GameFramework;
using System;

namespace Server
{
	internal sealed class NetworkEventManager
	{
		private readonly ConcurrentDoubleKeyMap<long, int, ConcurrentHashSet<object>> _protoHandlerDataMap = new();
		private readonly ConcurrentQueue<(long ConnectionId, ProtoObject Data)> _networkMessageQueue = new();
		private readonly SynchronizationContext _unitySynchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
		internal void Update()
		{
			while (_networkMessageQueue.Count > 0)
			{
				if (_networkMessageQueue.TryDequeue(out var message))
				{
					_unitySynchronizationContext.Send((object state) =>
					{
						HandleNetworkEvent(message.ConnectionId, message.Data);
					}, null);

				}
			}
		}

		internal void Shutdown()
		{
			_protoHandlerDataMap.Clear();
			_networkMessageQueue.Clear();
		}

		public void Listen<T>(long connectionID, int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				if (!_protoHandlerDataMap.TryGetValue(connectionID, id, out var handlerList))
				{
					handlerList = new();
					_protoHandlerDataMap.Add(connectionID, id, handlerList);
				}
				handlerList.Add(handler);
			}, null);
		}

		public void Unlisten<T>(long connectionID, int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				if (_protoHandlerDataMap.TryGetValue(connectionID, id, out var handlerList))
				{
					handlerList.Remove(handler, out _);
					if (handlerList.Count <= 0)
					{
						_protoHandlerDataMap.Remove(connectionID, id);
					}
				}
			}, null);
		}

		public void Trigger<T>(long connectionId, T e) where T : ProtoObject
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				_networkMessageQueue.Enqueue((connectionId, e));
			}, null);
		}

		public void TriggerNow<T>(long connectionId, T e) where T : ProtoObject
		{
			
			_unitySynchronizationContext.Send((object state) =>
			{
				HandleNetworkEvent(connectionId, e);
			}, null);
		}

		private void HandleNetworkEvent(long connectionId, ProtoObject data)
		{
			if (_protoHandlerDataMap.TryGetValue(connectionId, data.ProtocolID, out var handlerList))
			{
				foreach (var handler in handlerList.Values)
				{
					if(handler is Delegate d)
					{
						d.DynamicInvoke(data);
					}
				}
			}
			data.Recycle();
		}
	}
}
