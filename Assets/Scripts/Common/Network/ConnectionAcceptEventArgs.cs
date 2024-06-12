using GameFramework;
using GameFramework.Event;

namespace Common
{
	public class ConnectionAcceptEventArgs : GameEventArgs
	{
		public const string EventId = "Common.ConnectionAcceptEventArgs";

		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public static ConnectionAcceptEventArgs Create()
		{
			return ReferencePool.Acquire<ConnectionAcceptEventArgs>();
		}

		public override void Clear()
		{
			
		}
	}
}
