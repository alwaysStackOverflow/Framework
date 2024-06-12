using UnityGameFramework;
using Common;

namespace Client
{
	public class MenuProcedure : AProcedure
	{
		protected override void OnEnter()
		{
			ModuleManager.Instance.AwakeModule(ModuleType.Menu, ModuleType.Login);
			Subscribe<InitServerFinshEvent>(InitServerFinshEvent.EventId, OnInitServerFinsh);
			Subscribe<ConnectionAcceptEventArgs>(ConnectionAcceptEventArgs.EventId, OnConnectAccept);
			Listen<CheckAccountReply>(Protocol.CheckAccountReply, OnCheckAccountReply);
			OpenUIMenuEvent.Fire();
		}

		protected override void OnLeave(bool isShutdown)
		{
			Unsubscribe<InitServerFinshEvent>(InitServerFinshEvent.EventId, OnInitServerFinsh);
			Subscribe<ConnectionAcceptEventArgs>(ConnectionAcceptEventArgs.EventId, OnConnectAccept);
			Unlisten<CheckAccountReply>(Protocol.CheckAccountReply, OnCheckAccountReply);
		}

		private void OnInitServerFinsh(InitServerFinshEvent e)
		{
			Log.Info($"InitServerFinsh");
			ClientEntry.Network.Connect(e.IPEndPointV6, e.IPEndPointV4);
			ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model.Token = e.Token;

		}

		public void OnConnectAccept(ConnectionAcceptEventArgs e)
		{
			Log.Info("Connected");
			var request = CreateProtocolObject<CheckAccountRequest>();
			request.Token = ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model.Token;
			SendNetworkMessage(request);
		}

		public void OnCheckAccountReply(CheckAccountReply data)
		{
			if(data.Code != ProtocolCode.OK)
			{
				Log.Error($"Login Error Code:{data.Code}");
				return;
			}
			ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model.HasMainAccount = data.MainAccountExist;
			ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model.IsMainAccount = true;
			ModuleManager.Instance.ResetData(ModuleType.Menu);
			DestroyUIMenuEvent.Fire();
			ChangeProcedure<LoginProcedure>();
		}
	}
}
