using GameFramework;

namespace Client
{
	public class OpenUICreateNewPlayerForm : GameEventArgs
	{
		public const string EventId = "Client.OpenUICreateNewPlayerForm";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static void Fire()
		{
			var arg = ReferencePool.Acquire<OpenUICreateNewPlayerForm>();
			arg.Invoke();
		}
	}

	public class OnCloseUICreateNewPlayerForm : GameEventArgs
	{
		public const string EventId = "Client.OnCloseUICreateNewPlayerForm";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static OnCloseUICreateNewPlayerForm Create()
		{
			return ReferencePool.Acquire<OnCloseUICreateNewPlayerForm>();
		}
	}
}
