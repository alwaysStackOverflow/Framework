﻿using UnityGameFramework;
using GameFramework.Procedure;
using GameFramework.Resource;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Client
{
	public class MenuProcedure : ProcedureBase
	{
		private ProcedureOwner _owner;
		private bool _changeScene = false;

		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			base.OnInit(procedureOwner);
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{

			base.OnEnter(procedureOwner);
			_owner = procedureOwner;
			_changeScene = false;

			//ClientEntry.Event.Subscribe()
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
			
		}
	}
}
