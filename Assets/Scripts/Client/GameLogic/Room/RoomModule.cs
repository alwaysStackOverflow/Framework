namespace Client
{
	public class RoomModule : AModule<RoomData,RoomController>
	{
		public override ModuleType ModuleType => ModuleType.Room;

		public override void RegisterViewConfig()
		{
		}
	}
}
