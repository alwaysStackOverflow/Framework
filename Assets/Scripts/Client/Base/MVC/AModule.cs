using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityGameFramework;

namespace Client
{
	public interface IModule
	{
		public void Init();
		public void Release();
		public void Awake();
		public void Shutdown();
		public void Update(float elapseSeconds, float realElapseSeconds);
		public abstract ModuleType ModuleType { get; }
		public abstract GameEventArgs GetViewDestroyEventArg(Type t);
		public abstract void ResetData();
	}


	public abstract class AModule<M, C> : IModule where M : AModel where C : AController
	{
		public abstract ModuleType ModuleType { get; }
		private Dictionary<Type, object> _gameEvents;
		public M Model { get; private set; }
		public C Controller { get; private set; }
		public AModule()
		{
			Model = Activator.CreateInstance<M>();
			Controller = Activator.CreateInstance<C>();
			Controller.Module = this;
			_gameEvents = new ();
		}
		public virtual void Init()
		{
			Model?.ResetData();
			Controller?.Init();
			RegisterViewConfig();
		}

		public abstract void RegisterViewConfig();

		public void RegisterView<T, A>(GameFrameworkFunc<A> arg) where T : UIBaseForm where A : GameEventArgs
		{
			_gameEvents.Add(typeof(T), arg);
		}

		public virtual void Release()
		{
			_gameEvents.Clear();
			Shutdown();
			Model = null;
			Controller = null;
			_gameEvents.Clear();
		}
		public virtual void Update(float elapseSeconds, float realElapseSeconds)
		{
			Controller.OnUpdate(elapseSeconds, realElapseSeconds);
		}

		public GameEventArgs GetViewDestroyEventArg(Type t)
		{
			if (_gameEvents.TryGetValue(t, out var obj))
			{
				if (obj is Delegate d)
				{
					return d.DynamicInvoke() as GameEventArgs;
				}
			}
			return null;
		}

		public void ResetData()
		{
			Model?.ResetData();
		}

		public void Awake()
		{
			Controller?.Awake();
		}

		public void Shutdown()
		{
			Model?.ResetData();
			Controller?.Shutdown();
		}
	}
}
