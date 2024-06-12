using System;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Client;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;
using FilePath = Sirenix.OdinInspector.FilePathAttribute;
using Common;

[CustomEditor(typeof(ResourceConfig), true)]
[CanEditMultipleObjects]
public class ResourceConfigInspector : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		GUILayout.Label("在  项目工具/Resources Config 配置工具  的窗口编辑该文件");
		serializedObject.ApplyModifiedProperties();
	}
}

public class ResourceConfigInspectorWindow : OdinEditorWindow
{
	[MenuItem("项目工具/Resources Config 配置工具")]
	public static void ShowWindow()
	{
		var window = GetWindow<ResourceConfigInspectorWindow>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 1000);
	}

	private enum DataType
	{
		Scene,
		UI,
		PlayerModel,
		WeaponModel,
		Building,
	}

	[SerializeField]
	private DataType _currentDataType;

	[Serializable]
	public class SceneDataInfo
	{
		public SceneType SceneType;
		[FilePath(ParentFolder = ResourceConfig.SceneParentConstPath, Extensions = ".unity")]//多个拓展名过滤用;隔开
		public string AssetsPath;
	}

	[Serializable]
	public class UIDataInfo
	{
		public UIType UIType;
		[FilePath(ParentFolder = ResourceConfig.UIParentConstPath, Extensions = ".prefab")]//多个拓展名过滤用;隔开
		public string AssetsPath;
		public UILayer UILayer;
	}

	[Serializable]
	public class PlayerModelInfo
	{
		public PlayerModelType Type;
		[FilePath(ParentFolder = ResourceConfig.UIParentConstPath, Extensions = ".prefab")]//多个拓展名过滤用;隔开
		public string AssetPath;
	}

	[Serializable]
	public class WeaponModelInfo
	{
		public WeaponType Type;
		[FilePath(ParentFolder = ResourceConfig.UIParentConstPath, Extensions = ".prefab")]//多个拓展名过滤用;隔开
		public string AssetPath;
	}

	[Serializable]
	public class BuildingModelInfo
	{
		public BuildingType Type;
		[FilePath(ParentFolder = ResourceConfig.UIParentConstPath, Extensions = ".prefab")]//多个拓展名过滤用;隔开
		public string AssetPath;
	}

	[SerializeField]
	[ShowIf("@_currentDataType == DataType.Scene")]
	[ListDrawerSettings(NumberOfItemsPerPage = 20, ShowPaging = true, Expanded = true)]
	private List<SceneDataInfo> _SceneConfig = new();

	[SerializeField]
	[ShowIf("@_currentDataType == DataType.UI")]
	[ListDrawerSettings(NumberOfItemsPerPage = 14, ShowPaging = true, Expanded = true)]
	private List<UIDataInfo> _UIConfig = new();

	[SerializeField]
	[ShowIf("@_currentDataType == DataType.PlayerModel")]
	[ListDrawerSettings(NumberOfItemsPerPage = 20, ShowPaging = true, Expanded = true)]
	private List<PlayerModelInfo> _PlayerConfig = new();

	[SerializeField]
	[ShowIf("@_currentDataType == DataType.WeaponModel")]
	[ListDrawerSettings(NumberOfItemsPerPage = 20, ShowPaging = true, Expanded = true)]
	private List<WeaponModelInfo> _WeaponConfig = new();

	[SerializeField]
	[ShowIf("@_currentDataType == DataType.Building")]
	[ListDrawerSettings(NumberOfItemsPerPage = 20, ShowPaging = true, Expanded = true)]
	private List<BuildingModelInfo> _BuildingConfig = new();


	private ResourceConfig _config;

	protected override void OnEnable()
	{
		base.OnEnable();
		_currentDataType = DataType.Scene;
		_config = AssetDatabase.LoadAssetAtPath<ResourceConfig>("Assets/Bundles/Config/ResourceConfig.asset");
		_SceneConfig.Clear();
		_UIConfig.Clear();
		_PlayerConfig.Clear();
		_WeaponConfig.Clear();
		_BuildingConfig.Clear();
		foreach (var scene in _config.SceneConfig)
		{
			var s = new SceneDataInfo()
			{
				SceneType = scene.SceneType,
			};
			if (scene.AssetsPath.Contains(ResourceConfig.SceneParentConstPath))
			{
				s.AssetsPath = scene.AssetsPath[(scene.AssetsPath.IndexOf(ResourceConfig.SceneParentConstPath) + ResourceConfig.SceneParentConstPath.Length + 1)..];
			}
			else
			{
				s.AssetsPath = scene.AssetsPath;
			}
			_SceneConfig.Add(s);
		}
		foreach(var ui in _config.UIConfig)
		{
			var s = new UIDataInfo()
			{
				UIType = ui.UIType,
				UILayer = ui.UILayer
			};
			if (ui.AssetsPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetsPath = ui.AssetsPath[(ui.AssetsPath.IndexOf(ResourceConfig.UIParentConstPath) + ResourceConfig.UIParentConstPath.Length + 1)..];
			}
			else
			{
				s.AssetsPath = ui.AssetsPath;
			}
				_UIConfig.Add(s);
		}
		foreach (var player in _config.PlayerModelConfig)
		{
			var s = new PlayerModelInfo()
			{
				Type = player.Type,
			};
			if (player.AssetPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetPath = player.AssetPath[(player.AssetPath.IndexOf(ResourceConfig.UIParentConstPath) + ResourceConfig.UIParentConstPath.Length + 1)..];
			}
			else
			{
				s.AssetPath = player.AssetPath;
			}
			_PlayerConfig.Add(s);
		}
		foreach (var weapon in _config.WeaponModelConfig)
		{
			var s = new WeaponModelInfo()
			{
				Type = weapon.Type,
			};
			if (weapon.AssetPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetPath = weapon.AssetPath[(weapon.AssetPath.IndexOf(ResourceConfig.UIParentConstPath) + ResourceConfig.UIParentConstPath.Length + 1)..];
			}
			else
			{
				s.AssetPath = weapon.AssetPath;
			}
			_WeaponConfig.Add(s);
		}
		foreach (var building in _config.BuildingModelConfig)
		{
			var s = new BuildingModelInfo()
			{
				Type = building.Type,
			};
			if (building.AssetPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetPath = building.AssetPath[(building.AssetPath.IndexOf(ResourceConfig.UIParentConstPath) + ResourceConfig.UIParentConstPath.Length + 1)..];
			}
			else
			{
				s.AssetPath = building.AssetPath;
			}
			_BuildingConfig.Add(s);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Save();
	}

	[Button("保存（关闭窗口后会自动保存）", buttonSize:ButtonSizes.Large)]
	private void Save()
	{
		_config.SceneConfig.Clear();
		_config.UIConfig.Clear();
		_config.PlayerModelConfig.Clear();
		_config.WeaponModelConfig.Clear();
		_config.BuildingModelConfig.Clear();
		foreach (var scene in _SceneConfig)
		{
			if (string.IsNullOrWhiteSpace(scene.AssetsPath))
			{
				continue;
			}
			var s = new ResourceConfig.SceneDataInfo()
			{
				SceneType = scene.SceneType,
			};
			if (scene.AssetsPath.Contains(ResourceConfig.SceneParentConstPath))
			{
				s.AssetsPath = scene.AssetsPath[scene.AssetsPath.IndexOf(ResourceConfig.SceneParentConstPath)..];
			}
			else
			{
				s.AssetsPath = $"{ResourceConfig.SceneParentConstPath}/{scene.AssetsPath}";
			}
			_config.SceneConfig.Add(s);
		}

		foreach (var ui in _UIConfig)
		{
			if (string.IsNullOrWhiteSpace(ui.AssetsPath))
			{
				continue;
			}
			var s = new ResourceConfig.UIDataInfo()
			{
				UIType = ui.UIType,
				UILayer = ui.UILayer
			};
			if (ui.AssetsPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetsPath = ui.AssetsPath[ui.AssetsPath.IndexOf(ResourceConfig.UIParentConstPath)..];
			}
			else
			{
				s.AssetsPath = $"{ResourceConfig.UIParentConstPath}/{ui.AssetsPath}";
			}
			_config.UIConfig.Add(s);
		}

		foreach (var player in _PlayerConfig)
		{
			if (string.IsNullOrWhiteSpace(player.AssetPath))
			{
				continue;
			}
			var s = new ResourceConfig.PlayerModelInfo()
			{
				Type = player.Type,
			};
			if (player.AssetPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetPath = player.AssetPath[player.AssetPath.IndexOf(ResourceConfig.UIParentConstPath)..];
			}
			else
			{
				s.AssetPath = $"{ResourceConfig.UIParentConstPath}/{player.AssetPath}";
			}
			_config.PlayerModelConfig.Add(s);
		}

		foreach (var weapon in _WeaponConfig)
		{
			if (string.IsNullOrWhiteSpace(weapon.AssetPath))
			{
				continue;
			}
			var s = new ResourceConfig.WeaponModelInfo()
			{
				Type = weapon.Type,
			};
			if (weapon.AssetPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetPath = weapon.AssetPath[weapon.AssetPath.IndexOf(ResourceConfig.UIParentConstPath)..];
			}
			else
			{
				s.AssetPath = $"{ResourceConfig.UIParentConstPath}/{weapon.AssetPath}";
			}
			_config.WeaponModelConfig.Add(s);
		}

		foreach (var building in _BuildingConfig)
		{
			if (string.IsNullOrWhiteSpace(building.AssetPath))
			{
				continue;
			}
			var s = new ResourceConfig.BuildingModelInfo()
			{
				Type = building.Type,
			};
			if (building.AssetPath.Contains(ResourceConfig.UIParentConstPath))
			{
				s.AssetPath = building.AssetPath[building.AssetPath.IndexOf(ResourceConfig.UIParentConstPath)..];
			}
			else
			{
				s.AssetPath = $"{ResourceConfig.UIParentConstPath}/{building.AssetPath}";
			}
			_config.BuildingModelConfig.Add(s);
		}

		EditorUtility.SetDirty(_config);
		AssetDatabase.SaveAssets();
	}
}
