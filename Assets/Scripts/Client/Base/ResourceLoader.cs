using Common;
using Loader;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;
using UnityObject = UnityEngine.Object;

namespace Client
{
	public class ResourceLoader
	{
		public static void UnloadUnusedAssets()
		{
			ResourceManager.UnloadUnusedAssets();
		}

		public static async Task DownloadAll()
		{
			await ResourceManager.DownloadAll();
		}

		public static async Task DownloadWithTag(params string[] tags)
		{
			await ResourceManager.DownloadWithTag(tags);
		}

		public static async Task DownloadWithAssetsInfo(params AssetInfo[] assetInfos)
		{
			await ResourceManager.DownloadWithAssetsInfo(assetInfos);
		}

		public static async Task<GameObject> LoadGameObjectAsync(UnityObject parent, UIType resourceType)
		{
			var config = ResourceConfig.Get(resourceType);
			return await ResourceManager.LoadGameObjectAsync(parent, config.AssetsPath);
		}

		public static async Task<GameObject> LoadGameObjectAsync(UnityObject parent, string path)
		{
			return await ResourceManager.LoadGameObjectAsync(parent, path);
		}

		public static async Task<T> LoadAssetAsync<T>(UIType resourceType) where T : UnityObject
		{
			var config = ResourceConfig.Get(resourceType);
			return await ResourceManager.LoadAssetAsync<T>(config.AssetsPath);
		}

		public static async Task<T> LoadSubAssetAsync<T>(string path, string assetName) where T : UnityObject
		{
			return await ResourceManager.LoadSubAssetAsync<T>(path, assetName);
		}

		public static async Task LoadSceneAsync(SceneType resourceType)
		{
			var config = ResourceConfig.Get(resourceType);
			await ResourceManager.LoadSceneAsync(config.AssetsPath);
		}

		public static async Task<T> LoadConfig<T>(ConfigType type) where T : ScriptableObject
		{
			var configInfo = ConfigList.Get(type);
			return await ResourceManager.LoadConfig<T>(configInfo.ConfigPath);
		}

		public static async Task<Dictionary<string, T>> LoadAllAssetsAsync<T>(string path) where T : UnityObject
		{
			return await ResourceManager.LoadAllAssetsAsync<T>(path);
		}

		public static AssetInfo[] GetAssetInfosByTag(string tag)
		{
			return ResourceManager.GetAssetInfosByTag(tag);
		}
	}
}
