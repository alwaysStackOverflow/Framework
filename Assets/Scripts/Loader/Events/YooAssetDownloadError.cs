using GameFramework.Event;
using GameFramework;

namespace Loader
{
	internal class YooAssetDownloadError : GameEventArgs
	{
		public const string EventId = "Loader.YooAssetDownloadError";
		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public string FileName;

		public string Error;

		public static YooAssetDownloadError Create(string fileName, string error)
		{
			var yooAssetDownloadStart = ReferencePool.Acquire<YooAssetDownloadError>();
			yooAssetDownloadStart.Error = error;
			yooAssetDownloadStart.FileName = fileName;
			return yooAssetDownloadStart;
		}

		public override void Clear()
		{
			FileName = null;
			Error = null;
		}
	}
}
