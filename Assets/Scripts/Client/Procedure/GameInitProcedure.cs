using Common;

namespace Client
{
	public class GameInitProcedure : AProcedure
	{
		private bool _initialized = false;

		protected override async void OnEnter()
		{
			await ResourceConfig.Init();
			await ResourceLoader.LoadSceneAsync(SceneType.Menu);
			_initialized = true;
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			if (_initialized)
			{
				ChangeProcedure<MenuProcedure>();
			}
		}

		protected override void OnLeave(bool isShutdown)
		{
			_initialized = false;
		}
	}
}
