using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using UnityObject = UnityEngine.Object;
using System.Collections.Generic;
using Common;

public class DefineModify : OdinEditorWindow
{
	[MenuItem("项目工具/宏定义工具")]
	public static void ShowWindow()
	{
		var window = GetWindow<DefineModify>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 610);
	}


	private static readonly BuildTargetGroup[] BuildTargetGroups = new BuildTargetGroup[]
		{
			BuildTargetGroup.Standalone,
			BuildTargetGroup.iOS,
			BuildTargetGroup.Android,
			BuildTargetGroup.WSA,
			BuildTargetGroup.WebGL,
		};

	private static readonly string[] AssemblyPath = new string[]
	{
		"Assets/Scripts/Framework/GameFramework/GameFramework.asmdef",
		"Assets/Scripts/Framework/YooAsset/YooAsset.asmdef",
		"Assets/Scripts/Framework/UnityGameFramework/UnityGameFramework.asmdef",
		"Assets/Scripts/Loader/Loader.asmdef",
		"Assets/Scripts/Common/Common.asmdef",
		"Assets/Scripts/Client/Client.asmdef",
		"Assets/Scripts/Server/Server.asmdef",
		"Assets/Scripts/Editor/Editor.asmdef",
		"Assets/Scripts/Editor/YooAssetEditor/YooAsset.Editor.asmdef",
	};

	[Serializable]
	public class AssemblyDefinitionFile
	{
		public string name;
		public string rootNamespace;
		public string[] references;
		public string[] includePlatforms;
		public string[] excludePlatforms;
		public bool allowUnsafeCode;
		public bool overrideReferences;
		public string[] precompiledReferences;
		public bool autoReferenced;
		public string[] defineConstraints;
		public string[] versionDefines;
		public bool noEngineReferences;
	}

	/// <summary>
	/// 检查指定平台是否存在指定的脚本宏定义。
	/// </summary>
	/// <param name="buildTargetGroup">要检查脚本宏定义的平台。</param>
	/// <param name="defineSymbol">要检查的脚本宏定义。</param>
	/// <returns>指定平台是否存在指定的脚本宏定义。</returns>
	public static bool HasDefineSymbol(BuildTargetGroup buildTargetGroup, string defineSymbol)
	{
		if (string.IsNullOrEmpty(defineSymbol))
		{
			return false;
		}

		string[] defineSymbols = GetScriptingDefineSymbols(buildTargetGroup);
		foreach (string define in defineSymbols)
		{
			if (define == defineSymbol)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 为指定平台增加指定的脚本宏定义。
	/// </summary>
	/// <param name="buildTargetGroup">要增加脚本宏定义的平台。</param>
	/// <param name="defineSymbol">要增加的脚本宏定义。</param>
	public static void AddDefineSymbol(BuildTargetGroup buildTargetGroup, string defineSymbol)
	{
		if (string.IsNullOrEmpty(defineSymbol))
		{
			return;
		}

		if (HasDefineSymbol(buildTargetGroup, defineSymbol))
		{
			return;
		}

		List<string> defineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup))
			{
				defineSymbol
			};

		SetDefineSymbols(buildTargetGroup, defineSymbols.ToArray());
	}

	/// <summary>
	/// 为指定平台移除指定的脚本宏定义。
	/// </summary>
	/// <param name="buildTargetGroup">要移除脚本宏定义的平台。</param>
	/// <param name="defineSymbol">要移除的脚本宏定义。</param>
	public static void RemoveDefineSymbol(BuildTargetGroup buildTargetGroup, string defineSymbol)
	{
		if (string.IsNullOrEmpty(defineSymbol))
		{
			return;
		}

		if (!HasDefineSymbol(buildTargetGroup, defineSymbol))
		{
			return;
		}

		List<string> defineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup));
		while (defineSymbols.Contains(defineSymbol))
		{
			defineSymbols.Remove(defineSymbol);
		}

		SetDefineSymbols(buildTargetGroup, defineSymbols.ToArray());
	}

	/// <summary>
	/// 获取指定平台的脚本宏定义。
	/// </summary>
	/// <param name="buildTargetGroup">要获取脚本宏定义的平台。</param>
	/// <returns>平台的脚本宏定义。</returns>
	public static string[] GetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup)
	{
		return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
	}

	/// <summary>
	/// 设置指定平台的脚本宏定义。
	/// </summary>
	/// <param name="buildTargetGroup">要设置脚本宏定义的平台。</param>
	/// <param name="scriptingDefineSymbols">要设置的脚本宏定义。</param>
	public static void SetDefineSymbols(BuildTargetGroup buildTargetGroup, string[] scriptingDefineSymbols)
	{
		PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", scriptingDefineSymbols));
	}

	public static AssemblyDefinitionFile GetAssemblyDefinitionFile(string assemblyPath)
	{
		if (string.IsNullOrEmpty(assemblyPath))
		{
			return new AssemblyDefinitionFile();
		}
		if (!File.Exists(assemblyPath))
		{
			return new AssemblyDefinitionFile();
		}
		return JsonHelper.Deserialize<AssemblyDefinitionFile>(File.ReadAllText(assemblyPath));
	}

	public static bool HasAssemblyDefineSymbol(string assemblyPath, string defineSymbol)
	{
		if (string.IsNullOrEmpty(defineSymbol))
		{
			return false;
		}
		var info = GetAssemblyDefinitionFile(assemblyPath);
		foreach (var define in info.defineConstraints)
		{
			if (define == defineSymbol)
			{
				return true;
			}
		}
		return false;
	}

	public static void AddAssemblyDefineSymbol(string assemblyPath, string defineSymbol)
	{
		if (string.IsNullOrEmpty(defineSymbol))
		{
			return;
		}
		if (string.IsNullOrEmpty(assemblyPath))
		{
			return;
		}
		if (!File.Exists(assemblyPath))
		{
			return;
		}

		if (HasAssemblyDefineSymbol(assemblyPath, defineSymbol))
		{
			return;
		}
		var info = GetAssemblyDefinitionFile(assemblyPath);
		var list = new List<string>(info.defineConstraints)
		{
			defineSymbol
		};
		info.defineConstraints = list.ToArray();
		File.WriteAllText(assemblyPath, JsonHelper.Serialize(info));
	}

	public static void RemoveAssemblyDefineSymbol(string assemblyPath, string defineSymbol)
	{
		if (string.IsNullOrEmpty(defineSymbol))
		{
			return;
		}
		if (string.IsNullOrEmpty(assemblyPath))
		{
			return;
		}
		if (!File.Exists(assemblyPath))
		{
			return;
		}

		if (!HasAssemblyDefineSymbol(assemblyPath, defineSymbol))
		{
			return;
		}
		var info = GetAssemblyDefinitionFile(assemblyPath);
		var list = new List<string>(info.defineConstraints);
		list.Remove(defineSymbol);
		info.defineConstraints = list.ToArray();
		File.WriteAllText(assemblyPath, JsonHelper.Serialize(info));
	}

	/// <summary>
	/// 为所有平台增加指定的脚本宏定义。
	/// </summary>
	/// <param name="defineSymbol">要增加的脚本宏定义。</param>
	public static void AddScriptingDefineSymbol(string defineSymbol)
	{
		foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
		{
			AddDefineSymbol(buildTargetGroup, defineSymbol);
		}
	}

	/// <summary>
	/// 为所有平台移除指定的脚本宏定义。
	/// </summary>
	/// <param name="defineSymbol">要移除的脚本宏定义。</param>
	public static void RemoveScriptingDefineSymbol(string defineSymbol)
	{
		foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
		{
			RemoveDefineSymbol(buildTargetGroup, defineSymbol);
		}

	}

	public static void AddAssemblyDefineSymbol(string defineSymbol)
	{
		foreach (var path in AssemblyPath)
		{
			AddAssemblyDefineSymbol(path, defineSymbol);
		}
		AssetDatabase.Refresh();
	}

	public static void RemoveAssemblyDefineSymbol(string defineSymbol)
	{
		foreach (var path in AssemblyPath)
		{
			RemoveAssemblyDefineSymbol(path, defineSymbol);
		}
		AssetDatabase.Refresh();
	}

	public static void AddDefine(string define)
	{
		AddScriptingDefineSymbol(define);
		AddAssemblyDefineSymbol(define);
	}

	public static void RemoveDefine(string define)
	{
		RemoveAssemblyDefineSymbol(define);
		RemoveScriptingDefineSymbol(define);
	}

	public List<string> defines = new();

	[Button("添加宏定义", ButtonSizes.Large)]
	public void AddDefineSymbolBtn()
	{
		foreach(var define in defines)
		{
			AddDefine(define);
		}

	}

	[Button("移除宏定义", ButtonSizes.Large)]
	public void RemoveDefineSymbolBtn()
	{
		foreach (var define in defines)
		{
			RemoveDefine(define);
		}
	}


	[Button("添加 启用热更 宏定义", ButtonSizes.Large)]
	public void AddHotUpdateDefineSymbolBtn()
	{
		var define = "ENABLE_HOTUPDATE";
		var path = "Assets/Scripts/Loader/Loader.asmdef";
		AddScriptingDefineSymbol(define);
		AddAssemblyDefineSymbol(path, define);
		AssetDatabase.Refresh();
	}

	[Button("移除 启用热更 宏定义", ButtonSizes.Large)]
	public void RemoveHotUpdateDefineSymbolBtn()
	{
		var define = "ENABLE_HOTUPDATE";
		var path = "Assets/Scripts/Loader/Loader.asmdef";
		RemoveAssemblyDefineSymbol(define);
		AssetDatabase.Refresh();
		RemoveAssemblyDefineSymbol(path, define);
	}

	[Button("添加 启用IL2CPP 宏定义", ButtonSizes.Large)]
	public void AddIL2CPPDefineSymbolBtn()
	{
		AddDefine("ENABLE_IL2CPP");
	}

	[Button("移除 启用IL2CPP 宏定义", ButtonSizes.Large)]
	public void RemoveIL2CPPDefineSymbolBtn()
	{
		RemoveDefine("ENABLE_IL2CPP");
	}
}
