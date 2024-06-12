using GameFramework;
using GameFramework.Event;
using System.Diagnostics;
using UnityGameFramework;

namespace Common
{
	public class AwakeServerEvent : GameEventArgs
	{
		public const string EventId = "Common.AwakeServerEvent";

		public AwakeServerEvent()
		{
			Database = null;
		}

		public override string Id
		{
			get
			{
				return EventId;
			}
		}

		public string Database {  get; private set; }

		public static void Fire(string database)
		{
			var arg = ReferencePool.Acquire<AwakeServerEvent>();
			arg.Database = database;
			CommonEntry.Event.Fire(arg);
		}

		public override void Clear()
		{
			Database = null;
		}
	}
}
