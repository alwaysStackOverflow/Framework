using GameFramework;
using GameFramework.Event;
using System.Net;

namespace Common
{
	public class InitServerFinshEvent : GameEventArgs
	{
		public const string EventId = "Common.InitServerFinshEvent";

		public InitServerFinshEvent()
		{
		}

		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public IPEndPoint IPEndPointV6 { get; set; }
		public IPEndPoint IPEndPointV4 { get; set; }
		public string Token { get; set; }

		public static void Fire(IPEndPoint iPEndPointV6, IPEndPoint iPEndPointV4, string token)
		{
			var arg = ReferencePool.Acquire<InitServerFinshEvent>();
			arg.IPEndPointV6 = iPEndPointV6;
			arg.IPEndPointV4 = iPEndPointV4;
			arg.Token = token;
			CommonEntry.Event.Fire(arg);
		}

		public override void Clear()
		{
			IPEndPointV6 = null;
			IPEndPointV4 = null;
			Token = null;
		}
	}
}
