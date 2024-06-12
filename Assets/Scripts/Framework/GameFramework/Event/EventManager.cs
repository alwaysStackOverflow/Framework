using System.Threading;
using GameFramework.Network;
using System.Collections.Concurrent;

namespace GameFramework.Event
{
	internal sealed class EventManager : GameFrameworkModule, IEventManager
	{
		private class EventData : IReference
		{
			public BaseEventArgs Data { get; set; }

			public void Clear()
			{
				Data = null;
			}

			public static EventData Create(BaseEventArgs data)
			{
				var eventNode = ReferencePool.Acquire<EventData>();
				eventNode.Data = data;
				return eventNode;
			}
		}

		private class NetworkData : IReference
		{
			public ProtoObject Data { get; set; }

			public void Clear()
			{
				Data = null;
			}

			public static NetworkData Create(ProtoObject data)
			{
				var eventNode = ReferencePool.Acquire<NetworkData>();
				eventNode.Data = data;
				return eventNode;
			}
		}

		internal override int Priority => EventManagerPriority;

		private readonly ConcurrentDoubleKeyMap<string, object, AActionArgsData> _eventHandlerDataMap = new();
		private readonly ConcurrentQueue<EventData> _eventsQueue = new();

		private readonly ConcurrentDoubleKeyMap<int, object, AActionArgsData> _protoHandlerDataMap = new();
		private readonly ConcurrentQueue<NetworkData> _networkMessageQueue = new();
		private SynchronizationContext _unitySynchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
		internal override void Update(float elapseSeconds, float realElapseSeconds)
		{
			while (_networkMessageQueue.Count > 0)
			{
				if (_networkMessageQueue.TryDequeue(out var eventNode))
				{
					_unitySynchronizationContext.Send((object state) =>
					{
						HandleNetworkEvent(eventNode.Data);
						ReferencePool.Release(eventNode);
					}, null);

				}
			}

			while (_eventsQueue.Count > 0)
			{
				if (_eventsQueue.TryDequeue(out var eventNode))
				{
					_unitySynchronizationContext.Send((object state) =>
					{
						HandleEvent(eventNode.Data);
						ReferencePool.Release(eventNode);
					}, null);
				}
			}
		}

		internal override void Shutdown()
		{
			_protoHandlerDataMap.Clear();
			_networkMessageQueue.Clear();
			_eventHandlerDataMap.Clear();
			_eventsQueue.Clear();
		}


		#region Event
		public void Subscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
			var handlerData = ActionArgsData<T>.Create(handler);
			_eventHandlerDataMap.Add(id, handler, handlerData);
		}

		public void Unsubscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
			_eventHandlerDataMap.Remove(id, handler);
		}

		public void Fire<T>(T e = default) where T : BaseEventArgs
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				var eventNode = EventData.Create(e);
				_eventsQueue.Enqueue(eventNode);
			}, null);
		}

		public void FireNow<T>(T e = default) where T : BaseEventArgs
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				HandleEvent(e);
			}, null);
		}

		private void HandleEvent(BaseEventArgs e)
		{
			if(_eventHandlerDataMap.TryGetValue(e.Id, out var handlerList))
			{
				foreach(var handlerData in handlerList.Values)
				{
					handlerData.Invoke(e);
				}
			}
			ReferencePool.Release(e);
		}
		#endregion Event

		#region NetworkMessage
		public void Listen<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			var hendlerData = ActionArgsData<T>.Create(handler);
			_protoHandlerDataMap.Add(id, handler, hendlerData);
		}

		public void Unlisten<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			_protoHandlerDataMap.Remove(id, handler);
		}

		public void Trigger<T>(T e) where T : ProtoObject
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				var eventNode = NetworkData.Create(e);
				_networkMessageQueue.Enqueue(eventNode);
			}, null);
		}

		public void TriggerNow<T>(T e) where T : ProtoObject
		{
			_unitySynchronizationContext.Send((object state) =>
			{
				HandleNetworkEvent(e);
			}, null);
		}

		private void HandleNetworkEvent(ProtoObject data)
		{
			if (_protoHandlerDataMap.TryGetValue(data.ProtocolID, out var handlerList))
			{
				foreach (var handlerData in handlerList.Values)
				{
					handlerData.Invoke(data);
				}
			}
			data.Recycle();
		}

		#endregion NetworkMessage
	}
}
