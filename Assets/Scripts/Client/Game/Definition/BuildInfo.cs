using System;
using UnityEngine;

namespace Client
{
	[Serializable]
	[CreateAssetMenu(menuName = "项目配置信息/生成BuildInfo文件", fileName = "BuildInfo", order = 0)]
	public class BuildInfo : ScriptableObject
    {
		public string GameVersion;

		public int InternalGameVersion;

		public string CheckVersionUrl;

		public string WindowsAppUrl;

		public string AndroidAppUrl;
	}
}
