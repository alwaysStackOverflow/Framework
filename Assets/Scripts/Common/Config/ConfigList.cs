using Loader;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
	public enum ConfigType
	{
		None,
		ConfigList,
		ResourceConfig,
		PlayerModelConfig,
		BuildingConfig,
		LevelConfig,
	}

	[CreateAssetMenu(menuName = "项目配置信息创建/生成ConfigList文件", fileName = "ConfigList", order = 0)]
	public class ConfigList : AConfig
	{
		[Serializable]
		public class ConfigInfo
		{
			public ConfigType ConfigType;
			public string ConfigPath;
		}

		public List<ConfigInfo> ConfigInfoList;

		private static bool _inited;
		private readonly static Dictionary<ConfigType, ConfigInfo> _configDic = new();

		public static async Task Init()
		{
			if (_inited)
			{
				return;
			}
			var config = await ResourceManager.LoadConfig<ConfigList>("Assets/Bundles/Config/ConfigList.asset");
			foreach(var info in config.ConfigInfoList)
			{
				_configDic.Add(info.ConfigType, info);
			}
			_inited = true;
		}

		public static ConfigInfo Get(ConfigType type)
		{
			if(_configDic.TryGetValue(type, out var info))
			{
				return info;
			}
			return null;
		}
	}
}
