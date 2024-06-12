using System;
using GameFramework;
using UnityGameFramework;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Threading.Tasks;
using System.Reflection;

namespace Loader
{
	public class AppRunPurcedure : ProcedureBase
	{
		private Type _clientStartPurcedure = null;

		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			Log.Info("Init AppRunPurcedure");
			base.OnInit(procedureOwner);
		}

		protected override async void OnEnter(ProcedureOwner procedureOwner)
		{
			Log.Info("Enter AppRunPurcedure");
			base.OnEnter(procedureOwner);
			await DllManager.Init();
			await InitConfig();
			InitClientPurcedure();
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
			if (_clientStartPurcedure != null && LoaderEntry.Procedure.HasProcedure(_clientStartPurcedure))
			{
				ChangeProcedure(_clientStartPurcedure);
			}
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			Log.Info("Leave AppRunPurcedure");
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}

		private async Task InitConfig(Assembly assembly)
		{
			foreach (var type in assembly.GetTypes())
			{
				if (type.IsSubclassOf(typeof(AConfig)))
				{
					try
					{
						var initMethodInfo = type.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
						if(initMethodInfo != null)
						{
							await (Task)initMethodInfo.Invoke(null, null);
							Log.Info($"{type} Init");
						}
						else
						{
							Log.Warning($"{type} Init Function Not Find");
						}
					}
					catch(Exception e)
					{
						Log.Error($"Type:{type} {e.Message}\n{e.StackTrace}");
						return;
					}
				}
			}
		}

		private async Task InitConfig()
		{
			if (Utility.Assembly.CommonAssembly != null)
			{
				await InitConfig(Utility.Assembly.CommonAssembly);
			}
			if (Utility.Assembly.ClientAssembly != null)
			{
				await InitConfig(Utility.Assembly.ClientAssembly);
			}
			if (Utility.Assembly.ServerAssembly != null)
			{
				await InitConfig(Utility.Assembly.ServerAssembly);
			}
		}

		public void InitClientPurcedure()
		{
			if(Utility.Assembly.ClientAssembly != null)
			{
				foreach(var type in Utility.Assembly.ClientAssembly.GetTypes())
				{
					if (type.IsSubclassOf(typeof(ProcedureBase)) && !type.IsAbstract)
					{
						var procedure = (ProcedureBase)Activator.CreateInstance(type);
						LoaderEntry.Procedure.AddProcedure(procedure);
						if(type.FullName == "Client.GameInitProcedure")
						{
							_clientStartPurcedure =	type;
						}
					}
				}
			}
		}
	}

}