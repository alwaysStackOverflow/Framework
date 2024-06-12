using GameFramework;
using GameFramework.Event;

namespace Common
{
	public class ShutdownServerEvent : GameEventArgs
	{
		public const string EventId = "Common.ShutdownServerEvent";

		public ShutdownServerEvent()
		{
			
		}

		public override string Id
		{
			get
			{
				return EventId;
			}
		}
		public static void Fire()
		{
			var arg = ReferencePool.Acquire<ShutdownServerEvent>();
			CommonEntry.Event.Fire(arg);
		}

		public override void Clear()
		{
			
		}
	}
}
