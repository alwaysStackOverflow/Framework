using UnityGameFramework;

namespace Client
{
	public class PlayerController : AController
	{
		public PlayerData Data { get; private set; }
		private UIPlayerInfo _playerView;

		protected override void OnInit()
		{
			Data = (Module as PlayerModule).Model;
		}

		protected override void OnAwake()
		{
			Subscribe<OpenUIPlayerInfoEvent>(OpenUIPlayerInfoEvent.EventId, OnOpenUIPlayerInfo);
			Subscribe<DestroyUIPlayerInfoEvent>(DestroyUIPlayerInfoEvent.EventId, OnDestroyUIPlayerInfo);
			Subscribe<OnCloseUIPlayerInfoEvent>(OnCloseUIPlayerInfoEvent.EventId, OnCloseUIPlayerInfo);
		}

		protected override void OnShutdown()
		{
			Unsubscribe<OpenUIPlayerInfoEvent>(OpenUIPlayerInfoEvent.EventId, OnOpenUIPlayerInfo);
			Unsubscribe<DestroyUIPlayerInfoEvent>(DestroyUIPlayerInfoEvent.EventId, OnDestroyUIPlayerInfo);
			Unsubscribe<OnCloseUIPlayerInfoEvent>(OnCloseUIPlayerInfoEvent.EventId, OnCloseUIPlayerInfo);
		}

		private async void OnOpenUIPlayerInfo(OpenUIPlayerInfoEvent e)
		{
			if(_playerView != null)
			{
				return;
			}
			_playerView = await CreateView<UIPlayerInfo>();
		}

		private void OnDestroyUIPlayerInfo(DestroyUIPlayerInfoEvent e)
		{
			_playerView?.Destroy();
		}

		private void OnCloseUIPlayerInfo(OnCloseUIPlayerInfoEvent e)
		{
			_playerView = null;
		}
	}
}
