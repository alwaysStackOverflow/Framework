using UnityGameFramework;
using Common;

namespace Client
{
	public class LoginProcedure : AProcedure
	{
		protected override void OnEnter()
		{
			ModuleManager.Instance.AwakeModule(ModuleType.Player);
			Listen<AccountLoginReply>(Protocol.AccountLoginReply, OnAccountLoginReply);
			if (ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model.HasMainAccount)
			{
				SendLogin();
			}
			else
			{
				OpenUICreateNewPlayerForm.Fire();
			}
		}

		protected override void OnLeave(bool isShutdown)
		{
			Unlisten<AccountLoginReply>(Protocol.AccountLoginReply, OnAccountLoginReply);
			ModuleManager.Instance.ShutdownModule(ModuleType.Menu, ModuleType.Login);
		}

		private async void OnAccountLoginReply(AccountLoginReply data)
		{
			if (data.Code != ProtocolCode.OK)
			{
				Log.Error($"Login Error Code:{data.Code}");
				return;
			}
			var playerData = ModuleManager.Instance.GetModule<PlayerModule>(ModuleType.Player).Model;
			playerData.UID = data.UID;
			playerData.Name = data.Name;
			playerData.Account = data.Account;
			playerData.Password = data.Password;
			playerData.Gender = data.Gender;
			playerData.ModelType = data.ModelType;
			playerData.Life = data.Life;
			playerData.Hunger = data.Hunger;
			playerData.Thirst = data.Thirst;
			playerData.Level = data.Level;
			playerData.Exp = data.Exp;
			playerData.NextLevelExp = data.NextLevelExp;
			playerData.Scene = data.Scene;
			FormManager.Instance.CloseAllForm();
			await ResourceLoader.LoadSceneAsync(SceneType.Home);
			ResourceLoader.UnloadUnusedAssets();
			ChangeProcedure<RoomProcedure>();
		}

		private void SendLogin()
		{
			var request = CreateProtocolObject<MainAccountLoginRequest>();
			request.Token = ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model.Token;
			SendNetworkMessage(request);
		}
	}
}
