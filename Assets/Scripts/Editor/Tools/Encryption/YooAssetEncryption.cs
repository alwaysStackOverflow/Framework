using System;
using System.IO;
using YooAsset;
using Loader;

public class YooAssetEncryption
{
	/// <summary>
	/// 文件偏移加密方式
	/// </summary>
	public class FileOffsetEncryption : IEncryptionServices
	{
		public EncryptResult Encrypt(EncryptFileInfo fileInfo)
		{
			//DefineModify.AddDefine("FILE_OFFSET_ENCRYPTION");
			byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
			var encryptedData = new byte[fileData.Length + YooAssetDecryption.FileOffsetDecryption.FileOffset];
			Buffer.BlockCopy(fileData, 0, encryptedData, YooAssetDecryption.FileOffsetDecryption.FileOffset, fileData.Length);

			EncryptResult result = new()
			{
				Encrypted = true,
				EncryptedData = encryptedData
			};
			return result;
		}
	}

	//public class MemoryEncryption : IEncryptionServices
	//{
	//	public EncryptResult Encrypt(EncryptFileInfo fileInfo)
	//	{
	//		int offset = 32;
	//		byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
	//		var encryptedData = new byte[fileData.Length + offset];
	//		Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);

	//		EncryptResult result = new()
	//		{
	//			Encrypted = true,
	//			EncryptedData = encryptedData
	//		};
	//		return result;
	//	}
	//}

	//public class StreamEncryption : IEncryptionServices
	//{
	//	public EncryptResult Encrypt(EncryptFileInfo fileInfo)
	//	{
	//		int offset = 32;
	//		byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
	//		var encryptedData = new byte[fileData.Length + offset];
	//		Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);

	//		EncryptResult result = new()
	//		{
	//			Encrypted = true,
	//			EncryptedData = encryptedData
	//		};
	//		return result;
	//	}
	//}
}