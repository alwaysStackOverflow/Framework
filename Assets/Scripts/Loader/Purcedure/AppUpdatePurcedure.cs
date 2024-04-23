using UnityGameFramework;
using GameFramework.Procedure;
using GameFramework.Localization;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Loader
{
	public class AppUpdatePurcedure : ProcedureBase
	{
		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			Log.Info("Init AppUpdatePurcedure");
			base.OnInit(procedureOwner);
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{
			Log.Info("Enter AppUpdatePurcedure");
			base.OnEnter(procedureOwner);
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			Log.Info("Leave AppUpdatePurcedure");
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
			// TODO: 这里可以播放一个 Splash 动画
			// ...


		}
	}
}
