using System;
using UnityEngine;

namespace Client
{
	[Serializable]
	[CreateAssetMenu(menuName = "项目配置信息创建/生成AppInfo文件", fileName = "AppInfo", order = 0)]
	public class AppInfo : ScriptableObject
    {
		public string GameVersion;

		public int InternalGameVersion;

		public string CheckVersionUrl;

		public string WindowsAppUrl;

		public string AndroidAppUrl;
	}
}
