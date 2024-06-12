using GameFramework;
using GameFramework.Event;

namespace Common
{
	public class NetworkErrorEventArgs : GameEventArgs
	{
		public const string EventId = "Common.NetworkErrorEventArgs";

		public override string Id
		{
			get
			{
				return EventId;
			}
		}
		public int Code { get; set; }

		public static NetworkErrorEventArgs Create(int code)
		{
			var obj = ReferencePool.Acquire<NetworkErrorEventArgs>();
			obj.Code = code;
			return obj;
		}

		public override void Clear()
		{
			Code = 0;
		}
	}
}