using GameFramework;

namespace Client
{
	public class OpenUIPlayerInfoEvent : GameEventArgs
	{
		public const string EventId = "Client.OpenUIPlayerInfoEvent";
		public override string Id => EventId;

		public override void Clear()
		{
			
		}

		public static void Fire()
		{
			var arg = ReferencePool.Acquire<OpenUIPlayerInfoEvent>();
			arg.Invoke();
		}
	}

	public class DestroyUIPlayerInfoEvent : GameEventArgs
	{
		public const string EventId = "Client.DestroyUIPlayerInfoEvent";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static void Fire()
		{
			var arg = ReferencePool.Acquire<DestroyUIPlayerInfoEvent>();
			arg.Invoke();
		}
	}

	public class OnCloseUIPlayerInfoEvent : GameEventArgs
	{
		public const string EventId = "Client.OnCloseUIPlayerInfoEvent";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static OnCloseUIPlayerInfoEvent Create()
		{
			return ReferencePool.Acquire<OnCloseUIPlayerInfoEvent>();
		}
	}
}
