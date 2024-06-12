using System;
using UnityGameFramework;
using GameFramework.Singleton;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Client
{
	public class ModuleManager : Singleton<ModuleManager>
	{
		public override int Priority => 1;

		private ConcurrentDictionary<ModuleType, IModule> _awakedModuleList;
		private ConcurrentDictionary<ModuleType, IModule> _shutdownModuleList;
		private HashSet<Type> _moduleTypes;

		public override void Init()
		{
			_awakedModuleList = new();
			_shutdownModuleList = new();
			_moduleTypes = new();
			RegisterModule<MenuModule>(false);
			RegisterModule<LoginModule>(false);
			RegisterModule<PlayerModule>(false);
			RegisterModule<RoomModule>(false);
		}

		public override void Release()
		{
			foreach (var module in _awakedModuleList)
			{
				module.Value.Release();
			}
			foreach (var module in _shutdownModuleList)
			{
				module.Value.Release();
			}
			_awakedModuleList.Clear();
			_shutdownModuleList.Clear();
			_moduleTypes.Clear();
			_awakedModuleList = null;
			_shutdownModuleList = null;
			_moduleTypes = null;
		}

		public void AwakeModule(params ModuleType[] types)
		{
			foreach(var type in types)
			{
				if (_shutdownModuleList.TryRemove(type, out var module))
				{
					module.Awake();
					_awakedModuleList.TryAdd(type, module);
				}
			}

		}

		public void ShutdownModule(params ModuleType[] types)
		{
			foreach (var type in types)
			{
				if (_awakedModuleList.TryRemove(type, out var module))
				{
					module.Shutdown();
					_shutdownModuleList.TryAdd(type, module);
				}
			}
		}

		public void ResetData()
		{
			foreach (var module in _awakedModuleList)
			{
				module.Value.ResetData();
			}
		}

		public void ResetData(ModuleType type)
		{
			if(_awakedModuleList.TryGetValue(type, out var module))
			{
				module.ResetData();
			}
		}

		public override void Update(float elapseSeconds, float realElapseSeconds)
		{
			foreach (var module in _awakedModuleList)
			{
				module.Value.Update(elapseSeconds, realElapseSeconds);
			}
		}

		public void RegisterModule<T>(bool awake) where T : class, IModule, new()
		{
			if (_moduleTypes.Contains(typeof(T)))
			{
				Log.Warning($"Already exist Module{typeof(T)}");
				return;
			}
			var module = new T();
			module.Init();
			if (awake)
			{
				module.Awake();
				_awakedModuleList.TryAdd(module.ModuleType, module);
			}
			else
			{
				_shutdownModuleList.TryAdd(module.ModuleType, module);
			}
		}

		public T GetModule<T>(ModuleType type) where T : class, IModule, new()
		{
			if (_awakedModuleList.TryGetValue(type, out var module))
			{
				return module as T;
			}
			return null;
		}
	}
}
