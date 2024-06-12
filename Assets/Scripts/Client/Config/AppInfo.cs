using System;
using UnityEngine;

namespace Client
{
	[Serializable]
	[CreateAssetMenu(menuName = "��Ŀ������Ϣ����/����AppInfo�ļ�", fileName = "AppInfo", order = 0)]
	public class AppInfo : ScriptableObject
    {
		public string GameVersion;

		public int InternalGameVersion;

		public string CheckVersionUrl;

		public string WindowsAppUrl;

		public string AndroidAppUrl;
	}
}
