using UnityGameFramework;
using GameFramework.Procedure;
using GameFramework.Localization;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Loader
{
	public class AppInitPurcedure : ProcedureBase
	{
		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			Log.Info("Init AppInitPurcedure");
			base.OnInit(procedureOwner);
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{
			Log.Info("Enter AppInitPurcedure");
			base.OnEnter(procedureOwner);
			LoaderResourceManager.Init();
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

			ChangeState<AppUpdatePurcedure>(procedureOwner);
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			Log.Info("Leave AppInitPurcedure");
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}
	}

}