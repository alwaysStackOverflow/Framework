using UnityGameFramework;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Loader
{
	public class AppInitPurcedure : ProcedureBase
	{
		private bool _resourceManagerInitFinish;
		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			Log.Info("Init AppInitPurcedure");
			base.OnInit(procedureOwner);
		}

		protected override async void OnEnter(ProcedureOwner procedureOwner)
		{
			Log.Info("Enter AppInitPurcedure");
			base.OnEnter(procedureOwner);
			await ResourceManager.Init();
			_resourceManagerInitFinish = true;
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
			if (_resourceManagerInitFinish)
			{
				ChangeProcedure<AppUpdatePurcedure>();
			}
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			Log.Info("Leave AppInitPurcedure");
			base.OnLeave(procedureOwner, isShutdown);
			_resourceManagerInitFinish = false;
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}
	}

}