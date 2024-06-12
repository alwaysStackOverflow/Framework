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

[CustomEditor(typeof(ConfigList), true)]
[CanEditMultipleObjects]
public class ConfigListInspector : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		GUILayout.Label("在  项目工具/Config List 配置工具  的窗口编辑该文件");
		serializedObject.ApplyModifiedProperties();
	}
}

public class ConfigListInspectorWindow : OdinEditorWindow
{
	[MenuItem("项目工具/Config List 配置工具")]
	public static void ShowWindow()
	{
		var window = GetWindow<ConfigListInspectorWindow>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 1000);
	}

	private const string ConfigParentPath = "Assets/Bundles/Config";

	[Serializable]
	public class ConfigInfo
	{
		public ConfigType ConfigType;
		[FilePath(ParentFolder = ConfigParentPath, Extensions = ".asset")]//多个拓展名过滤用;隔开
		public string ConfigPath;
	}

	[SerializeField]
	[ListDrawerSettings(NumberOfItemsPerPage = 20, ShowPaging = true, Expanded = true)]
	private List<ConfigInfo> _ConfigList = new();


	private ConfigList _config;

	protected override void OnEnable()
	{
		base.OnEnable();
		_config = AssetDatabase.LoadAssetAtPath<ConfigList>($"{ConfigParentPath}/ConfigList.asset");
		_ConfigList.Clear();
		foreach (var info in _config.ConfigInfoList)
		{
			var s = new ConfigInfo()
			{
				ConfigType = info.ConfigType,
			};
			if (info.ConfigPath.Contains(ConfigParentPath))
			{
				s.ConfigPath = info.ConfigPath[(info.ConfigPath.IndexOf(ConfigParentPath) + ConfigParentPath.Length + 1)..];
			}
			else
			{
				s.ConfigPath = info.ConfigPath;
			}
			_ConfigList.Add(s);
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
		_config.ConfigInfoList.Clear();
		foreach (var info in _ConfigList)
		{
			if (string.IsNullOrWhiteSpace(info.ConfigPath))
			{
				continue;
			}
			var s = new ConfigList.ConfigInfo()
			{
				ConfigType = info.ConfigType,
			};
			if (info.ConfigPath.Contains(ConfigParentPath))
			{
				s.ConfigPath = info.ConfigPath[info.ConfigPath.IndexOf(ConfigParentPath)..];
			}
			else
			{
				s.ConfigPath = $"{ConfigParentPath}/{info.ConfigPath}";
			}
			_config.ConfigInfoList.Add(s);
		}

		EditorUtility.SetDirty(_config);
		AssetDatabase.SaveAssets();
	}
}
