using UnityEngine;
using GameFramework;
using UnityGameFramework;

namespace Client
{
    public class BuiltinDataComponent : GameFrameworkComponent
	{
		[SerializeField]
		private TextAsset _defaultDictionaryTextAsset = null;

		[SerializeField]
		private UIUpdateResourceForm _updateResourceFormTemplate = null;

		[SerializeField]
		private BuildInfo _buildInfo = null;

		public BuildInfo BuildInfo { get => _buildInfo; }

		public UIUpdateResourceForm UpdateResourceFormTemplate
		{
			get => _updateResourceFormTemplate;
		}

		public void InitBuildInfo()
		{
			if (BuildInfo == null)
			{
				Log.Warning("Init Build info failure.");
				return;
			}
		}

		public void InitDefaultDictionary()
		{
			if (_defaultDictionaryTextAsset == null || string.IsNullOrEmpty(_defaultDictionaryTextAsset.text))
			{
				Log.Info("Default dictionary can not be found or empty.");
				return;
			}

			if (!ClientEntry.Localization.ParseData(_defaultDictionaryTextAsset.text))
			{
				Log.Warning("Parse default dictionary failure.");
				return;
			}
		}
	}
}
