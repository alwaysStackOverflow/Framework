using GameFramework.Event;
using GameFramework;

namespace Loader
{
	internal class YooAssetDownloadStart : GameEventArgs
	{
		public const string EventId = "Loader.YooAssetDownloadStart";
		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public string FileName;

		public long FileSize;

		public static YooAssetDownloadStart Create(string fileName, long fileSize)
		{
			var yooAssetDownloadStart = ReferencePool.Acquire<YooAssetDownloadStart>();
			yooAssetDownloadStart.FileSize = fileSize;
			yooAssetDownloadStart.FileName = fileName;
			return yooAssetDownloadStart;
		}

		public override void Clear()
		{
			FileName = null;
			FileSize = 0;
		}
	}
}
