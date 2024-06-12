namespace Client
{
	public class LoginController : AController
	{
		public LoginData Data { get; private set; }

		private UICreateNewPlayerForm _createNewPlayerView = null;

		protected override void OnInit()
		{
			Data = (Module as LoginModule).Model;
		}

		protected override void OnAwake()
		{
			Subscribe<OpenUICreateNewPlayerForm>(OpenUICreateNewPlayerForm.EventId, OnOpenUICreateNewPlayerForm);
			Subscribe<OnCloseUICreateNewPlayerForm>(OnCloseUICreateNewPlayerForm.EventId, CloseUICreateNewPlayerForm);
		}

		protected override void OnShutdown()
		{
			Unsubscribe<OpenUICreateNewPlayerForm>(OpenUICreateNewPlayerForm.EventId, OnOpenUICreateNewPlayerForm);
			Unsubscribe<OnCloseUICreateNewPlayerForm>(OnCloseUICreateNewPlayerForm.EventId, CloseUICreateNewPlayerForm);
		}

		private async void OnOpenUICreateNewPlayerForm(OpenUICreateNewPlayerForm e)
		{
			if(_createNewPlayerView != null)
			{
				return;
			}
			_createNewPlayerView = await CreateView<UICreateNewPlayerForm>();
		}

		private void CloseUICreateNewPlayerForm(OnCloseUICreateNewPlayerForm e)
		{
			_createNewPlayerView = null;
		}
	}
}
