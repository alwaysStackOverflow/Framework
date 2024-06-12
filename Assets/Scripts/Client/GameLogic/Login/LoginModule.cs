namespace Client
{
	public class LoginModule : AModule<LoginData, LoginController>
	{
		public override ModuleType ModuleType => ModuleType.Login;

		public override void RegisterViewConfig()
		{
			RegisterView<UICreateNewPlayerForm, OnCloseUICreateNewPlayerForm>(OnCloseUICreateNewPlayerForm.Create);
		}
	}
}
