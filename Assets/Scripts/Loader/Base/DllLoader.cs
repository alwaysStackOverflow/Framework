using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HybridCLR;
using UnityEngine;
using UnityGameFramework;

namespace Loader
{
	public class DllLoader
	{
		public const string HotUpdateDllDir = "Assets/Bundles/Binaries";

		public const string AotDllDir = "Assets/Bundles/Binaries/AotDll";

		public const string BuildOutputDir = "Temp/Bin/Debug";


		private Dictionary<string, TextAsset> _dlls = new();
		private Dictionary<string, TextAsset> _aotDlls = new();

		public async void DownloadAsync()
		{
#if !UNITY_EDITOR
			_dlls = await ResourcesComponent.Instance.LoadAllAssetsAsync<TextAsset>($"{HotUpdateDllDir}/Common.dll.bytes");
			_aotDlls = await ResourcesComponent.Instance.LoadAllAssetsAsync<TextAsset>($"{AotDllDir}/mscorlib.dll.bytes");
#endif
		}

		public void LoadAssemblies()
		{
		#if UNITY_EDITOR
			#if !ENABLE_IL2CPP
				foreach (var textAsset in _aotDlls.Values)
				{
					RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, HomologousImageMode.SuperSet);
				}
				byte[] commonAssBytes = File.ReadAllBytes(Path.Combine(HotUpdateDllDir, "Common.dll.bytes"));
				byte[] commonPdbBytes = File.ReadAllBytes(Path.Combine(HotUpdateDllDir, "Common.pdb.bytes"));
				byte[] clientAssBytes = File.ReadAllBytes(Path.Combine(HotUpdateDllDir, "Client.dll.bytes"));
				byte[] clientPdbBytes = File.ReadAllBytes(Path.Combine(HotUpdateDllDir, "Client.pdb.bytes"));
				byte[] serverAssBytes = File.ReadAllBytes(Path.Combine(HotUpdateDllDir, "Server.dll.bytes"));
				byte[] serverPdbBytes = File.ReadAllBytes(Path.Combine(HotUpdateDllDir, "Server.pdb.bytes"));

				Assembly.Load(commonAssBytes, commonPdbBytes);
				Assembly.Load(clientAssBytes, clientPdbBytes);
				Assembly.Load(serverAssBytes, serverPdbBytes);
			#endif
		#else
			byte[] commonAssBytes = _dlls["Common.dll"].bytes;
			byte[] commonPdbBytes = _dlls["Common.pdb"].bytes;
			byte[] clientAssBytes = _dlls["Client.dll"].bytes;
			byte[] clientPdbBytes = _dlls["Client.pdb"].bytes;
			byte[] serverAssBytes = _dlls["Server.dll"].bytes;
			byte[] serverPdbBytes = _dlls["Server.pdb"].bytes;

			Assembly.Load(commonAssBytes, commonPdbBytes);
			Assembly.Load(clientAssBytes, clientPdbBytes);
			Assembly.Load(serverAssBytes, serverPdbBytes);
		#endif
		}
	}
}