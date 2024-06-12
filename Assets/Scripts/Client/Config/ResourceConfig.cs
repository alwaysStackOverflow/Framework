using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Common;
using Loader;

namespace Client
{
	public enum UIType
	{
		None,
		CommonEntry,
		ClientEntry,
		ServerEntry,
		UIUpdateResourceForm,
		UIMenuForm,
		UISavingsForm,
		UICreateNewPlayerForm,
		UILoginForm,
		PlayerInfo,
	}

	[CreateAssetMenu(menuName = "项目配置信息创建/生成ResourceConfig文件", fileName = "ResourceConfig", order = 0)]
	public class ResourceConfig : AConfig
	{
		[Serializable]
		public class SceneDataInfo
		{
			public SceneType SceneType;
			public string AssetsPath;
		}

		[Serializable]
		public class UIDataInfo
		{
			public UIType UIType;
			public string AssetsPath;
			public UILayer UILayer;
		}

		[Serializable]
		public class PlayerModelInfo
		{
			public PlayerModelType Type;
			public string AssetPath;
		}

		[Serializable]
		public class WeaponModelInfo
		{
			public WeaponType Type;
			public string AssetPath;
		}

		[Serializable]
		public class BuildingModelInfo
		{
			public BuildingType Type;
			public string AssetPath;
		}

		public List<SceneDataInfo> SceneConfig;
		public List<UIDataInfo> UIConfig;
		public List<PlayerModelInfo> PlayerModelConfig;
		public List<WeaponModelInfo> WeaponModelConfig;
		public List<BuildingModelInfo> BuildingModelConfig;


		private static bool _inited = false;
		private static readonly Dictionary<SceneType, SceneDataInfo> _sceneInfos = new();
		private static readonly Dictionary<UIType, UIDataInfo> _uiInfos = new();
		private static readonly Dictionary<PlayerModelType, PlayerModelInfo> _playerInfos = new();
		private static readonly Dictionary<WeaponType, WeaponModelInfo> _weaponInfos = new();
		private static readonly Dictionary<BuildingType, BuildingModelInfo> _buildingInfos = new();

		public const string SceneParentConstPath = "Assets/Bundles/GameResources/Scenes";
		public const string UIParentConstPath = "Assets/Bundles/GameResources/PrefabAssets";

		public static async Task Init()
		{
			if (_inited)
			{
				return;
			}

			var config = await ResourceLoader.LoadConfig<ResourceConfig>(ConfigType.ResourceConfig);

			foreach (var info in config.SceneConfig)
			{
				_sceneInfos.Add(info.SceneType, info);
			}

			foreach (var info in config.UIConfig)
			{
				_uiInfos.Add(info.UIType, info);
			}

			foreach (var info in config.PlayerModelConfig)
			{
				_playerInfos.Add(info.Type, info);
			}

			foreach (var info in config.WeaponModelConfig)
			{
				_weaponInfos.Add(info.Type, info);
			}

			foreach (var info in config.BuildingModelConfig)
			{
				_buildingInfos.Add(info.Type, info);
			}
			_inited = true;
		}
		public static UIDataInfo Get(UIType type)
		{
			if (_uiInfos.TryGetValue(type, out var info))
			{
				return info;
			}
			return null;
		}

		public static SceneDataInfo Get(SceneType type)
		{
			if (_sceneInfos.TryGetValue(type, out var info))
			{
				return info;
			}
			return null;
		}

		public static PlayerModelInfo Get(PlayerModelType type)
		{
			if(_playerInfos.TryGetValue(type,out var info))
			{
				return info;
			}
			return null;
		}

		public static WeaponModelInfo Get(WeaponType type)
		{
			if(_weaponInfos.TryGetValue(type, out var info))
			{
				return info;
			}
			return null;
		}

		public static BuildingModelInfo Get(BuildingType type)
		{
			if(_buildingInfos.TryGetValue(type, out var info))
			{
				return info;
			}
			return null;
		}
	}
}
