using System;
using UnityEngine;

namespace Client
{
	[Serializable]
	[CreateAssetMenu(menuName = "��Ŀ������Ϣ/����BuildInfo�ļ�", fileName = "BuildInfo", order = 0)]
	public class BuildInfo : ScriptableObject
    {
		public string GameVersion;

		public int InternalGameVersion;

		public string CheckVersionUrl;

		public string WindowsAppUrl;

		public string AndroidAppUrl;
	}
}
