using GameFramework.Event;
using GameFramework;

namespace Loader
{
	internal class YooAssetDownloadOver : GameEventArgs
	{
		public const string EventId = "Loader.YooAssetDownloadOver";
		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public bool IsSucceed;

		public static YooAssetDownloadOver Create(bool isSucceed)
		{
			var yooAssetDownloadOver = ReferencePool.Acquire<YooAssetDownloadOver>();
			yooAssetDownloadOver.IsSucceed = isSucceed;
			return yooAssetDownloadOver;
		}

		public override void Clear()
		{
			IsSucceed = false;
		}
	}
}
