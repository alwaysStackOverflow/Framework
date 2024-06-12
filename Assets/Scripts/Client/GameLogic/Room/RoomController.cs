using Common;
using UnityGameFramework;

namespace Client
{
	public class RoomController : AController
	{
		public RoomData Data { get; private set; }
		private PlayerData _playerData;

		protected override void OnInit()
		{
			Data = (Module as RoomModule).Model;
		}

		protected override void OnAwake()
		{
			_playerData = ModuleManager.Instance.GetModule<PlayerModule>(ModuleType.Player).Model;
			RequestToEnterHome();
			Listen<EnterRoomReply>(Protocol.EnterRoomReply, OnEnterRoomReply);
		}

		protected override void OnShutdown()
		{
			Unlisten<EnterRoomReply>(Protocol.EnterRoomReply, OnEnterRoomReply);
		}

		private void RequestToEnterHome()
		{
			var request = CreateProtocolObject<EnterRoomRequest>();
			request.RoomType = SceneType.Home;
			request.UID = _playerData.UID;
			SendNetworkMessage(request);
		}

		private void OnEnterRoomReply(EnterRoomReply data)
		{
			Log.Info(data);
			Log.Info(data.Buildings);
		}
	}
}
