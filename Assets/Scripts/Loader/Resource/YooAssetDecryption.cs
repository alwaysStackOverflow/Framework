using System;
using System.IO;
using UnityEngine;
using YooAsset;

namespace Loader
{
	public class YooAssetDecryption
	{
		/// <summary>
		/// 文件偏移加密方式
		/// </summary>
		public class FileOffsetDecryption : IDecryptionServices
		{
			public const int FileOffset = 32;
			/// <summary>
			/// 同步方式获取解密的资源包对象
			/// 注意：加载流对象在资源包对象释放的时候会自动释放
			/// </summary>
			AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
			{
				managedStream = null;
				return AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.ConentCRC, FileOffset);
			}

			/// <summary>
			/// 异步方式获取解密的资源包对象
			/// 注意：加载流对象在资源包对象释放的时候会自动释放
			/// </summary>
			AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
			{
				managedStream = null;
				return AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.ConentCRC, FileOffset);
			}
		}

		//public class MemoryDecryption : IDecryptionServices
		//{
		//	/// <summary>
		//	/// 同步方式获取解密的资源包对象
		//	/// 注意：加载流对象在资源包对象释放的时候会自动释放
		//	/// </summary>
		//	AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
		//	{
		//		managedStream = null;
		//		return AssetBundle.LoadFromStream(managedStream, fileInfo.ConentCRC, (uint)managedStream.Position);
		//	}

		//	/// <summary>
		//	/// 异步方式获取解密的资源包对象
		//	/// 注意：加载流对象在资源包对象释放的时候会自动释放
		//	/// </summary>
		//	AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
		//	{
		//		managedStream = null;
		//		return AssetBundle.LoadFromStreamAsync(managedStream, fileInfo.ConentCRC, (uint)managedStream.Length);
		//	}
		//}

		//public class StreamDecryption : IDecryptionServices
		//{
		//	/// <summary>
		//	/// 同步方式获取解密的资源包对象
		//	/// 注意：加载流对象在资源包对象释放的时候会自动释放
		//	/// </summary>
		//	AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
		//	{
		//		managedStream = null;
		//		byte[] bytes = null;
		//		return AssetBundle.LoadFromMemory(bytes, fileInfo.ConentCRC);
		//	}

		//	/// <summary>
		//	/// 异步方式获取解密的资源包对象
		//	/// 注意：加载流对象在资源包对象释放的时候会自动释放
		//	/// </summary>
		//	AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
		//	{
		//		managedStream = null;
		//		byte[] bytes = null;
		//		return AssetBundle.LoadFromMemoryAsync(bytes, fileInfo.ConentCRC);
		//	}
		//}
	}
}
