using UnityEngine;
using YooAsset;

namespace Loader
{
	public enum EncryptionType
	{
		None,
		FileOffsetEncryption,
	}


	[CreateAssetMenu(menuName = "项目配置信息创建/生成GlobalConfig文件", fileName = "GlobalConfig", order = 0)]
	public class GlobalConfig : ScriptableObject
	{
		public string AppName;
		public bool IsTest;
		public EncryptionType EncryptionType;
		public EPlayMode AppRunMode;

		public const string DefaultYooFolderName = "Bundles"; //与YooAssetSettings的DefaultYooFolderName保持一致
	}
}
