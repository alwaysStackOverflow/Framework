using Common;
using Loader;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Server
{
	[CreateAssetMenu(menuName = "项目配置信息创建/生成LevelConfig文件", fileName = "LevelConfig", order = 0)]
	public class LevelConfig : AConfig
	{
		[Serializable]
		public class LevelInfo
		{
			public int Level;
			public long LevelMaxExp;
		}

		public List<LevelInfo> Levels;

		private static bool _inited = false;
		private readonly static Dictionary<int, LevelInfo> _infos = new();

		public static async Task Init()
		{
			if(_inited)
			{
				return;
			}
			var configInfo = ConfigList.Get(ConfigType.LevelConfig);
			var config = await ResourceManager.LoadConfig<LevelConfig>(configInfo.ConfigPath);
			foreach (var info in config.Levels)
			{
				_infos.Add(info.Level, info);
			}
			_inited = true;
		}

		public static LevelInfo Get(int level)
		{
			if(_infos.TryGetValue(level, out var info))
			{
				return info;
			}
			return null;
		}
	}
}
