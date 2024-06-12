using GameFramework.Timer;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityGameFramework;
using YooAsset;
using UnityObject = UnityEngine.Object;

namespace Loader
{
	/// <summary>
	/// 资源文件查询服务类
	/// </summary>
	public class BuildinQueryServices : IBuildinQueryServices
	{
		/// <summary>
		/// 查询内置文件的时候，是否比对文件哈希值
		/// </summary>
		public static bool CompareFileCRC = false;

		public bool Query(string packageName, string fileName, string fileCRC)
		{
			return true;
		}

		public static bool FileExists(string packageName, string fileName, string fileCRC32)
		{
			return true;
		}
	}

	public class RemoteServices : IRemoteServices
	{
		private readonly string _defaultHostServer;
		private readonly string _fallbackHostServer;

		public RemoteServices(string defaultHostServer, string fallbackHostServer)
		{
			_defaultHostServer = defaultHostServer;
			_fallbackHostServer = fallbackHostServer;
		}

		string IRemoteServices.GetRemoteMainURL(string fileName)
		{
			return $"{_defaultHostServer}/{fileName}";
		}

		string IRemoteServices.GetRemoteFallbackURL(string fileName)
		{
			return $"{_fallbackHostServer}/{fileName}";
		}
	}


	public static class ResourceManager
	{
		public static GlobalConfig GlobalConfig { get; private set; }
		private static string _packageName;
		private static ResourcePackage _package;
		private static string _packageVersion;
		public static async Task Init()
		{
			GlobalConfig = Resources.Load<GlobalConfig>("Config/GlobalConfig");
			YooAssets.Initialize();
			SetDefaultPackage();
			await InitializeYooAsset();
		}

		public static void SetDefaultPackage(string packageName = "DefaultPackage")
		{
			_packageName = packageName;
			_package = YooAssets.CreatePackage(packageName);
			YooAssets.SetDefaultPackage(_package);
		}

		public static void Release()
		{
			YooAssets.Destroy();
		}

		private static void Unload()
		{
			_package.UnloadUnusedAssets();
		}

		public static void UnloadUnusedAssets()
		{
			LoaderEntry.Timer.Remove("ResourceManager.UnloadUnusedAssets");
			var timer = Timer.Create("ResourceManager.UnloadUnusedAssets", 1, 5, false, Unload);
			LoaderEntry.Timer.Add(timer);
		}

		private static string GetHostServerURL()
		{
			if (GlobalConfig.IsTest)
			{
				return "http://127.0.0.1/Bundles";
			}
			else
			{
				return $"http://127.0.0.1/{GlobalConfig.AppName}/{GameFramework.Version.GameVersion}";
			}

		}

		private static async Task InitializeYooAsset()
		{
			var appRunMode = GlobalConfig.AppRunMode;
			switch (appRunMode)
			{
				case EPlayMode.EditorSimulateMode:
				{
					var initParameters = new EditorSimulateModeParameters
					{
						SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.ScriptableBuildPipeline, _packageName)
					};
					await _package.InitializeAsync(initParameters).Task;
					break;
				}
				case EPlayMode.OfflinePlayMode:
				{
					var initParameters = new OfflinePlayModeParameters();
					await _package.InitializeAsync(initParameters).Task;
					break;
				}
				case EPlayMode.HostPlayMode:
				{
					var initParameters = new HostPlayModeParameters
					{
						BuildinQueryServices = new BuildinQueryServices(),
						RemoteServices = new RemoteServices(GetHostServerURL(), GetHostServerURL())
					};
					switch (GlobalConfig.EncryptionType)
					{
						case EncryptionType.FileOffsetEncryption:
							initParameters.DecryptionServices = new YooAssetDecryption.FileOffsetDecryption();
							break;
					}
					await _package.InitializeAsync(initParameters).Task;
					break;
				}
				case EPlayMode.WebPlayMode:
				{
					var initParameters = new WebPlayModeParameters
					{
						BuildinQueryServices = new BuildinQueryServices(),
						RemoteServices = new RemoteServices(GetHostServerURL(), GetHostServerURL())
					};
					await _package.InitializeAsync(initParameters).Task;
					break;
				}
				default:
					Log.Error($"配置的模式错误，不支持模式: {appRunMode}");
					throw new ArgumentOutOfRangeException();
			}
		}

		public static async Task UpdatePackageVersion()
		{
			var operation = _package.UpdatePackageVersionAsync();
			await operation.Task;
			_packageVersion = operation.PackageVersion;
		}

		public static async Task UpdatePackageManifest()
		{
			await _package.UpdatePackageManifestAsync(_packageVersion).Task;
		}

		public static async Task DownloadAll()
		{
			int downloadingMaxNum = 10;
			int failedTryAgain = 3;
			await Download(_package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain));
		}

		public static async Task DownloadWithTag(params string[] tags)
		{
			int downloadingMaxNum = 10;
			int failedTryAgain = 3;
			await Download(_package.CreateResourceDownloader(tags, downloadingMaxNum, failedTryAgain));
		}

		public static async Task DownloadWithAssetsInfo(params AssetInfo[] assetInfos)
		{
			int downloadingMaxNum = 10;
			int failedTryAgain = 3;
			await Download(_package.CreateBundleDownloader(assetInfos, downloadingMaxNum, failedTryAgain));
		}

		private static async Task Download(ResourceDownloaderOperation downloader)
		{
			if (downloader.TotalDownloadCount == 0)
			{
				await Task.CompletedTask;
				OnDownloadOver(true);
				return;
			}

			//注册回调方法
			downloader.OnStartDownloadFileCallback = OnStartDownloadFile;
			downloader.OnDownloadProgressCallback = OnDownloadProgressUpdate;
			downloader.OnDownloadErrorCallback = OnDownloadError;
			downloader.OnDownloadOverCallback = OnDownloadOver;

			//开始下载
			downloader.BeginDownload();
			await downloader.Task;
		}

		private static ResourceDownloaderOperation CombineDownload(ResourceDownloaderOperation downloader1, ResourceDownloaderOperation downloader2)
		{
			downloader1.Combine(downloader2);
			return downloader1;
		}

		private static void OnStartDownloadFile(string fileName, long sizeBytes)
		{
			LoaderEntry.Event.Fire(YooAssetDownloadStart.Create(fileName, sizeBytes));
		}

		private static void OnDownloadProgressUpdate(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
		{
			LoaderEntry.Event.Fire(YooAssetDownloadProgressUpdate.Create(totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
		}

		private static void OnDownloadError(string fileName, string error)
		{
			LoaderEntry.Event.Fire(YooAssetDownloadError.Create(fileName, error));
		}

		private static void OnDownloadOver(bool isSucceed)
		{
			LoaderEntry.Event.Fire(YooAssetDownloadOver.Create(isSucceed));
		}

		public static async Task<GameObject> LoadGameObjectAsync(UnityObject parent, string path)
		{
			var handle = _package.LoadAssetAsync<GameObject>(path);
			await handle.Task;
			if (parent == null)
			{
				return handle.InstantiateSync(Vector3.zero, Quaternion.Euler(0, 0, 0));
			}
			if (parent is GameObject go)
			{
				return handle.InstantiateSync(Vector3.zero, Quaternion.Euler(0, 0, 0), go.transform);
			}
			else if (parent is Component comp)
			{
				return handle.InstantiateSync(Vector3.zero, Quaternion.Euler(0, 0, 0), comp.transform);
			}
			else
			{
				return handle.InstantiateSync(Vector3.zero, Quaternion.Euler(0, 0, 0));
			}
		}

		public static async Task<T> LoadAssetAsync<T>(string path) where T : UnityObject
		{
			var handle = _package.LoadAssetAsync<T>(path);
			await handle.Task;
			var obj = handle.GetAssetObject<T>();
			handle.Release();
			return UnityObject.Instantiate(obj);
		}

		public static async Task<T> LoadSubAssetAsync<T>(string path, string assetName) where T : UnityObject
		{
			var handle = _package.LoadSubAssetsAsync<Sprite>(path);
			await handle.Task;
			var obj = handle.GetSubAssetObject<T>(assetName);
			handle.Release();
			return UnityObject.Instantiate(obj);
		}

		public static async Task LoadSceneAsync(string path)
		{
			SceneHandle handle = _package.LoadSceneAsync(path, UnityEngine.SceneManagement.LoadSceneMode.Single, false);
			await handle.Task;

		}

		public static async Task<T> LoadConfig<T>(string path) where T : ScriptableObject
		{
			AssetHandle handle = _package.LoadAssetAsync(path);
			await handle.Task;
			var obj = handle.GetAssetObject<T>();
			handle.Release();
			return obj;
		}

		public static async Task<Dictionary<string, T>> LoadAllAssetsAsync<T>(string path) where T : UnityObject
		{
			var allAssetsOperationHandle = YooAssets.LoadAllAssetsAsync<T>(path);
			await allAssetsOperationHandle.Task;
			Dictionary<string, T> dictionary = new();
			foreach (var assetObj in allAssetsOperationHandle.AllAssetObjects)
			{
				T t = assetObj as T;
				dictionary.Add(t.name, t);
			}

			allAssetsOperationHandle.Release();
			return dictionary;
		}

		public static AssetInfo[] GetAssetInfosByTag(string tag)
		{
			return _package.GetAssetInfos(tag);
		}
	}

}