using GameFramework;
using GameFramework.Network;
using System;
using System.Threading.Tasks;
using UnityGameFramework;

namespace Client
{
	public abstract class AController
	{
		public IModule Module { get; set; }

		protected abstract void OnInit();

		protected abstract void OnAwake();

		protected abstract void OnShutdown();

		public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{

		}

		public void Init()
		{
			OnInit();
		}

		public void Awake()
		{
			OnAwake();
		}

		public void Shutdown()
		{
			OnShutdown();
		}

		public void Update(float elapseSeconds, float realElapseSeconds)
		{
			OnUpdate(elapseSeconds, realElapseSeconds);
		}

		public async Task<T> CreateView<T>() where T : UIBaseForm
		{
			var view = Activator.CreateInstance<T>();
			var a = Module.GetViewDestroyEventArg(typeof(T));
			view.DestroyEvent = a;
			await view.LoadAsync();
			return view;
		}

		public async Task<T> CreateView<T>(params object[] args)  where T : UIBaseForm
		{
			var view = Activator.CreateInstance(typeof(T), args) as T;
			view.DestroyEvent = Module.GetViewDestroyEventArg(typeof(T));
			await view.LoadAsync();
			return view;
		}

		public void Subscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
			ClientEntry.Event.Subscribe(id, handler);
		}

		public void Unsubscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
			ClientEntry.Event.Unsubscribe(id, handler);
		}

		public void Listen<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			ClientEntry.Event.Listen(id, handler);
		}

		public void Unlisten<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			ClientEntry.Event.Unlisten(id, handler);
		}

		public T CreateProtocolObject<T>() where T : ProtoObject, IReference, new()
		{
			var request = ReferencePool.Acquire<T>();
			request.Code = 0;
			return request;
		}

		public void SendNetworkMessage<T>(T request) where T : ProtoObject
		{
			ClientEntry.Network.Send(request);
		}
	}
}
