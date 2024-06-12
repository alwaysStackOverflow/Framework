using GameFramework.Event;
using GameFramework;

namespace Loader
{
	internal class YooAssetDownloadProgressUpdate : GameEventArgs
	{
		public const string EventId = "Loader.YooAssetDownloadProgressUpdate";
		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public int TotalDownloadCount;

		public int CurrentDownloadCount;

		public long TotalDownloadBytes;

		public long CurrentDownloadBytes;

		public static YooAssetDownloadProgressUpdate Create(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
		{
			var yooAssetDownloadProgressUpdate = ReferencePool.Acquire<YooAssetDownloadProgressUpdate>();
			yooAssetDownloadProgressUpdate.TotalDownloadCount = totalDownloadCount;
			yooAssetDownloadProgressUpdate.CurrentDownloadCount = currentDownloadCount;
			yooAssetDownloadProgressUpdate.CurrentDownloadBytes = currentDownloadBytes;
			yooAssetDownloadProgressUpdate.TotalDownloadBytes = totalDownloadBytes;
			return yooAssetDownloadProgressUpdate;
		}

		public override void Clear()
		{
			TotalDownloadCount = 0;
			CurrentDownloadCount = 0;
			CurrentDownloadBytes = 0;
			TotalDownloadBytes = 0;
		}
	}
}
