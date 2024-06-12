using GameFramework;
using GameFramework.Network;
using GameFramework.Procedure;
using UnityGameFramework;
using ProcedureManager = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Client
{
	public abstract class AProcedure : ProcedureBase
	{
		protected virtual void OnInit()
		{

		}
		protected abstract void OnEnter();

		protected abstract void OnLeave(bool isShutdown);

		protected virtual void OnDestroy()
		{

		}

		protected virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{

		}

		protected override void OnInit(ProcedureManager procedureOwner)
		{
			base.OnInit(procedureOwner);
			Log.Info($"Init {GetType()}");
			OnInit();
		}

		protected override void OnEnter(ProcedureManager procedureOwner)
		{
			base.OnEnter(procedureOwner);
			Log.Info($"Enter {GetType()}");
			OnEnter();
		}

		protected override void OnLeave(ProcedureManager procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);
			Log.Info($"Leave {GetType()}");
			OnLeave(isShutdown);
		}

		protected override void OnDestroy(ProcedureManager procedureOwner)
		{
			base.OnDestroy(procedureOwner);
			Log.Info($"Destroy {GetType()}");
			OnDestroy();
		}

		protected override void OnUpdate(ProcedureManager procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
			OnUpdate(elapseSeconds, realElapseSeconds);
		}

		public static void Listen<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			ClientEntry.Event.Listen(id, handler);
		}

		public static void Unlisten<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			ClientEntry.Event.Unlisten(id, handler);
		}

		public static void Subscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
			ClientEntry.Event.Subscribe(id, handler);
		}

		public static void Unsubscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
			ClientEntry.Event.Unsubscribe(id, handler);
		}

		public T CreateProtocolObject<T>() where T : ProtoObject, IReference, new()
		{
			var request = ReferencePool.Acquire<T>();
			request.Code = 0;
			return request;
		}

		public void SendNetworkMessage<T>(T request) where T : ProtoObject
		{
			ClientEntry.Network.Send(request);
		}
	}
}