using GameFramework;

namespace Client
{
	public class OpenUIMenuEvent : GameEventArgs
	{
		public const string EventId = "Client.OpenMenuEvent";
		public override string Id => EventId;

		public override void Clear()
		{
			
		}

		public static void Fire()
		{
			var arg = ReferencePool.Acquire<OpenUIMenuEvent>();
			arg.Invoke();
		}
	}

	public class DestroyUIMenuEvent : GameEventArgs
	{
		public const string EventId = "Client.DestroyUIMenuEvent";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static void Fire()
		{
			var arg = ReferencePool.Acquire<DestroyUIMenuEvent>();
			arg.Invoke();
		}
	}

	public class OnCloseUIMenuEvent : GameEventArgs
	{
		public const string EventId = "Client.OnCloseUIMenuEvent";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static OnCloseUIMenuEvent Create()
		{
			return ReferencePool.Acquire<OnCloseUIMenuEvent>();
		}
	}

	public class OpenUISavingsFormEvent : GameEventArgs
	{
		public const string EventId = "Client.OpenUISavingsFormEvent";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static void Fire()
		{
			var arg = ReferencePool.Acquire<OpenUISavingsFormEvent>();
			arg.Invoke();
		}
	}

	public class OnCloseUISavingsFormEvent : GameEventArgs
	{
		public const string EventId = "Client.OnCloseUISavingsFormEvent";
		public override string Id => EventId;

		public override void Clear()
		{

		}

		public static OnCloseUISavingsFormEvent Create()
		{
			return ReferencePool.Acquire<OnCloseUISavingsFormEvent>();
		}
	}
}
