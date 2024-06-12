using System;
using System.IO;
using HybridCLR;
using UnityEngine;
using GameFramework;
using System.Reflection;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;
using UnityGameFramework;

namespace Loader
{
	public class DllManager
	{
		public const string HotUpdateDllDir = "Assets/Bundles/Binaries";

		public const string AotDllDir = "Assets/Bundles/Binaries/AotDll";

		public const string BuildOutputDir = "Temp/Bin/Debug";


		private static Dictionary<string, TextAsset> _dlls = new();
		private static Dictionary<string, TextAsset> _aotDlls = new();

		public static async Task Init()
		{
#if !UNITY_EDITOR
			if (ResourceManager.GlobalConfig.AppRunMode != YooAsset.EPlayMode.EditorSimulateMode)
			{
				_dlls = await ResourceManager.LoadAllAssetsAsync<TextAsset>($"{HotUpdateDllDir}/Common.dll.bytes");
	#if ENABLE_IL2CPP
				_aotDlls = await ResourceManager.LoadAllAssetsAsync<TextAsset>($"{AotDllDir}/mscorlib.dll.bytes");
	#endif

			}
#else
			await Task.CompletedTask;
#endif
			LoadAssemblies();
		}

		private static void LoadAssemblies()
		{
			if (ResourceManager.GlobalConfig.AppRunMode != YooAsset.EPlayMode.EditorSimulateMode)
			{
#if UNITY_EDITOR
				byte[] commonAssBytes = File.ReadAllBytes($"{HotUpdateDllDir}/Common.dll.bytes");
				byte[] commonPdbBytes = File.ReadAllBytes($"{HotUpdateDllDir}/Common.pdb.bytes");
				byte[] clientAssBytes = File.ReadAllBytes($"{HotUpdateDllDir}/Client.dll.bytes");
				byte[] clientPdbBytes = File.ReadAllBytes($"{HotUpdateDllDir}/Client.pdb.bytes");
				byte[] serverAssBytes = File.ReadAllBytes($"{HotUpdateDllDir}/Server.dll.bytes");
				byte[] serverPdbBytes = File.ReadAllBytes($"{HotUpdateDllDir}/Server.pdb.bytes");
#else
				byte[] commonAssBytes = _dlls["Common.dll"].bytes;
				byte[] commonPdbBytes = _dlls["Common.pdb"].bytes;
				byte[] clientAssBytes = _dlls["Client.dll"].bytes;
				byte[] clientPdbBytes = _dlls["Client.pdb"].bytes;
				byte[] serverAssBytes = _dlls["Server.dll"].bytes;
				byte[] serverPdbBytes = _dlls["Server.pdb"].bytes;
	#if ENABLE_IL2CPP
				foreach (var textAsset in _aotDlls.Values)
				{
					RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, HomologousImageMode.SuperSet);
				}
	#endif

#endif
				Utility.Assembly.CommonAssembly = Assembly.Load(commonAssBytes, commonPdbBytes);
				Utility.Assembly.ClientAssembly = Assembly.Load(clientAssBytes, clientPdbBytes);
				Utility.Assembly.ServerAssembly = Assembly.Load(serverAssBytes, serverPdbBytes);
				Utility.Assembly.AddAssembliy(Utility.Assembly.CommonAssembly);
				Utility.Assembly.AddAssembliy(Utility.Assembly.ClientAssembly);
				Utility.Assembly.AddAssembliy(Utility.Assembly.ServerAssembly);
			}
		}
	}
}