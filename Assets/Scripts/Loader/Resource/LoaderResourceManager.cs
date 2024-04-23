using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Loader
{
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


	public class LoaderResourceManager
	{
		public static void Init()
		{
			YooAssets.Initialize();
		}

		public static void Release()
		{
			YooAssets.Destroy();
		}
	}

}