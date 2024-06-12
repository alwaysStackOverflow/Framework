
using Common;
using System;
using System.Collections.Generic;
using Loader;
using System.Threading.Tasks;
using UnityEngine;

namespace Server
{
	[CreateAssetMenu(menuName = "项目配置信息创建/生成BuildingConfig文件", fileName = "BuildingConfig", order = 0)]
	public class BuildingConfig : AConfig
	{
		[Serializable]
		public class BuildingInfo
		{
			public string Name_en;
			public string Name_zh;
			public BuildingType Type;
			public int Life;
			[SerializeField]
			public List<WeaponType> CanBeAttackedWeaponList;
			public HashSet<WeaponType> CanBeAttackedWeapon = new();

			public override string ToString()
			{
				return JsonHelper.Serialize(this);
			}
		}

		public List<BuildingInfo> Config;

		private static readonly Dictionary<BuildingType, BuildingInfo> _infos = new();

		public static async Task Init()
		{
			var configInfo = ConfigList.Get(ConfigType.BuildingConfig);
			var config = await ResourceManager.LoadConfig<BuildingConfig>(configInfo.ConfigPath);
			foreach (var info in config.Config)
			{
				info.CanBeAttackedWeapon.Clear();
				foreach (var weaponType in info.CanBeAttackedWeaponList)
				{
					info.CanBeAttackedWeapon.Add(weaponType);
				}
				_infos.Add(info.Type, info);
			}
		}
		public static BuildingInfo Get(BuildingType type)
		{
			if (_infos.TryGetValue(type, out var info))
			{
				return info;
			}
			return null;
		}
	}
}
