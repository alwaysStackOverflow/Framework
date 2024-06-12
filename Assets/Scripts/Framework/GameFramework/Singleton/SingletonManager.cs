using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = GameFramework.GameFrameworkLog;

namespace GameFramework.Singleton
{
	internal sealed class SingletonManager : GameFrameworkModule, ISingletonManager
	{
		internal override int Priority => SingletonManagerPriority;

		private readonly SortedList<int, ISingleton> _singletons = new();

		internal override void Shutdown()
		{
			foreach (var singleton in _singletons.Values)
			{
				singleton.Release();
			}
			_singletons.Clear();
		}

		internal override void Update(float elapseSeconds, float realElapseSeconds)
		{
			foreach(var singleton in _singletons.Values)
			{
				singleton.Update(elapseSeconds, realElapseSeconds);
			}
		}

		public void AddSingleton<T>() where T : Singleton<T>, new()
		{
			T singleton = new();
			singleton.Init();
			lock (this)
			{
				_singletons.Add(singleton.Priority, singleton);
			}
			singleton.Register();
		}
	}
}
