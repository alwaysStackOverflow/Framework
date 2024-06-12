using Common;

namespace Client
{
	public class RoomProcedure : AProcedure
	{
		protected override void OnEnter()
		{
			ModuleManager.Instance.AwakeModule(ModuleType.Room);
		}

		protected override void OnLeave(bool isShutdown)
		{
			ModuleManager.Instance.ShutdownModule(ModuleType.Room);
		}
	}
}
