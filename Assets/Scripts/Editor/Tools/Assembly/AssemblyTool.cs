using HybridCLR.Editor.Settings;
using Loader;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityGameFramework;

public class AssemblyTool
{
	public static readonly string[] DllNames = { "Common", "Client", "Server" };

	public static void DoCompile()
	{
		// 强制刷新一下，防止关闭auto refresh，编译出老代码
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		RefreshBuildType();

		bool isCompileOk = CompileDlls();
		if (!isCompileOk)
		{
			return;
		}

		CopyHotUpdateDlls();
		CopyAotDll();
		Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
		Log.Info($"Compile Finish!");
	}

	static void RefreshBuildType()
	{
		GlobalConfig globalConfig = Resources.Load<GlobalConfig>("Config/GlobalConfig");
		if (globalConfig)
		{
			EditorUserBuildSettings.development = globalConfig.IsTest;
		}
	}

	static bool CompileDlls()
	{
		bool isCompileOk = false;
		try
		{
			Directory.CreateDirectory(DllManager.BuildOutputDir);
			BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
			BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
			ScriptCompilationSettings scriptCompilationSettings = new()
			{
				group = group,
				target = target,
				extraScriptingDefines = new[] { "UNITY_COMPILE" },
				options = EditorUserBuildSettings.development ? ScriptCompilationOptions.DevelopmentBuild : ScriptCompilationOptions.None
			};
			ScriptCompilationResult result = PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, DllManager.BuildOutputDir);
			isCompileOk = result.assemblies.Count > 0;
			EditorUtility.ClearProgressBar();
		}
		catch
		{

		}
		return isCompileOk;
	}

	/// <summary>
	/// 将dll文件复制到加载目录
	/// </summary>
	private static void CopyHotUpdateDlls()
	{
		CleanDirectory(DllManager.HotUpdateDllDir);
		foreach (string dllName in DllNames)
		{
			string sourceDll = $"{DllManager.BuildOutputDir}/{dllName}.dll";
			string sourcePdb = $"{DllManager.BuildOutputDir}/{dllName}.pdb";
			File.Copy(sourceDll, $"{DllManager.HotUpdateDllDir}/{dllName}.dll.bytes", true);
			File.Copy(sourcePdb, $"{DllManager.HotUpdateDllDir}/{dllName}.pdb.bytes", true);
		}
		AssetDatabase.Refresh();
	}

	private static void CopyAotDll()
	{
		BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
		string fromDir = Path.Combine(HybridCLRSettings.Instance.strippedAOTDllOutputRootDir, target.ToString());
		string toDir = DllManager.AotDllDir;
		if (Directory.Exists(toDir))
		{
			Directory.Delete(toDir, true);
		}
		Directory.CreateDirectory(toDir);

		foreach (string aotDll in HybridCLRSettings.Instance.patchAOTAssemblies)
		{
			var file = Path.Combine(fromDir, aotDll);
			if (File.Exists(file))
			{
				File.Copy(file, Path.Combine(toDir, $"{aotDll}.bytes"), true);
			}
		}
		Log.Info($"CopyAotDll Finish!");
		AssetDatabase.Refresh();
	}

	private static void CleanDirectory(string dir)
	{
		if (!Directory.Exists(dir))
		{
			return;
		}
		foreach (string subdir in Directory.GetDirectories(dir))
		{
			Directory.Delete(subdir, true);
		}

		foreach (string subFile in Directory.GetFiles(dir))
		{
			File.Delete(subFile);
		}
	}
}
