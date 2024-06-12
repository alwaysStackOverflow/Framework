using UnityGameFramework;

namespace Client
{
	public class MenuController : AController
	{
		public MenuData Data { get; private set; }
		private UIMenu _menuView;
		private UISavingsForm _savingsView;

		protected override void OnInit()
		{
			Data = (Module as MenuModule).Model;
		}

		protected override void OnAwake()
		{
			Subscribe<OpenUIMenuEvent>(OpenUIMenuEvent.EventId, OnOpenUIMenu);
			Subscribe<DestroyUIMenuEvent>(DestroyUIMenuEvent.EventId, OnDestroyUIMenu);
			Subscribe<OnCloseUIMenuEvent>(OnCloseUIMenuEvent.EventId, OnCloseUIMenu);
			Subscribe<OpenUISavingsFormEvent>(OpenUISavingsFormEvent.EventId, OnOpenUISavingsForm);
			Subscribe<OnCloseUISavingsFormEvent>(OnCloseUISavingsFormEvent.EventId, OnCloseUISavingsForm);
		}

		protected override void OnShutdown()
		{
			Unsubscribe<OpenUIMenuEvent>(OpenUIMenuEvent.EventId, OnOpenUIMenu);
			Unsubscribe<DestroyUIMenuEvent>(DestroyUIMenuEvent.EventId, OnDestroyUIMenu);
			Unsubscribe<OnCloseUIMenuEvent>(OnCloseUIMenuEvent.EventId, OnCloseUIMenu);
			Unsubscribe<OpenUISavingsFormEvent>(OpenUISavingsFormEvent.EventId, OnOpenUISavingsForm);
			Unsubscribe<OnCloseUISavingsFormEvent>(OnCloseUISavingsFormEvent.EventId, OnCloseUISavingsForm);
		}

		private async void OnOpenUIMenu(OpenUIMenuEvent e)
		{
			if(_menuView != null)
			{
				return;
			}
			_menuView = await CreateView<UIMenu>();
		}

		private void OnDestroyUIMenu(DestroyUIMenuEvent e)
		{
			_menuView?.Destroy();
		}


		private void OnCloseUIMenu(OnCloseUIMenuEvent e)
		{
			_menuView = null;
		}

		private async void OnOpenUISavingsForm(OpenUISavingsFormEvent e)
		{
			if (_savingsView != null)
			{
				return;
			}
			_savingsView = await CreateView<UISavingsForm>();
		}
		private void OnCloseUISavingsForm(OnCloseUISavingsFormEvent e)
		{
			_savingsView = null;
		}
	}
}
