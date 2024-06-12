using UnityGameFramework;
using GameFramework.Procedure;
using GameFramework.Localization;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Loader
{
	public class AppUpdatePurcedure : ProcedureBase
	{
		private ProcedureOwner _procedureOwner;
		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			Log.Info("Init AppUpdatePurcedure");
			base.OnInit(procedureOwner);
		}

		protected override async void OnEnter(ProcedureOwner procedureOwner)
		{
			_procedureOwner = procedureOwner;
			Log.Info("Enter AppUpdatePurcedure");
			base.OnEnter(procedureOwner);
			LoaderEntry.Event.Subscribe<YooAssetDownloadStart>(YooAssetDownloadStart.EventId, OnStartDownloadFile);
			LoaderEntry.Event.Subscribe<YooAssetDownloadProgressUpdate>(YooAssetDownloadProgressUpdate.EventId, OnDownloadProgressUpdate);
			LoaderEntry.Event.Subscribe<YooAssetDownloadError>(YooAssetDownloadError.EventId, OnDownloadError);
			LoaderEntry.Event.Subscribe<YooAssetDownloadOver>(YooAssetDownloadOver.EventId, OnDownloadOver);
			await ResourceManager.UpdatePackageVersion();
			await ResourceManager.UpdatePackageManifest();
			await ResourceManager.DownloadAll();
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			Log.Info("Leave AppUpdatePurcedure");
			base.OnLeave(procedureOwner, isShutdown);
			LoaderEntry.Event.Unsubscribe<YooAssetDownloadStart>(YooAssetDownloadStart.EventId, OnStartDownloadFile);
			LoaderEntry.Event.Unsubscribe<YooAssetDownloadProgressUpdate>(YooAssetDownloadProgressUpdate.EventId, OnDownloadProgressUpdate);
			LoaderEntry.Event.Unsubscribe<YooAssetDownloadError>(YooAssetDownloadError.EventId, OnDownloadError);
			LoaderEntry.Event.Unsubscribe<YooAssetDownloadOver>(YooAssetDownloadOver.EventId, OnDownloadOver);
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

		}

		private void OnStartDownloadFile(YooAssetDownloadStart e)
		{
			Log.Info($"Start Download {e.FileName} Size: {e.FileSize / Kcp.OneK} K");
		}

		private void OnDownloadProgressUpdate(YooAssetDownloadProgressUpdate e)
		{
			Log.Info($"Progress: {e.CurrentDownloadCount / e.TotalDownloadCount} Size: {(e.CurrentDownloadBytes / Kcp.OneM)} M / { (e.TotalDownloadCount / Kcp.OneK)} M");
		}

		private void OnDownloadError(YooAssetDownloadError e)
		{
			Log.Info($"Download {e.FileName} Error Message: {e.Error}");
		}

		private void OnDownloadOver(YooAssetDownloadOver e)
		{
			if (e.IsSucceed)
			{
				Log.Info("更新完毕");
			}
			else
			{
				Log.Info("更新失败");
			}
			ChangeProcedure<AppRunPurcedure>();
		}
	}
}
