using System;

namespace GameFramework
{
	public static class TimeInfo
	{
		public static int TimeZone { get; private set; }
		public static DateTime LocalTime
		{
			get
			{
				return DateTime.Now;
			}
		}

		public static long LocalTimeTicks
		{
			get
			{
				return DateTime.Now.Ticks / 10000;
			}
		}

		#region ClientTools
		public static long Delay { get; set; }

		public static long ServerTimeOffset { private get; set; }

		public static void Init()
		{
			var span = DateTime.Now - DateTime.UtcNow;
			TimeZone = span.Hours;
			Update();
		}

		public static void Update()
		{
			ClientFrameStartTime = ClientTime;
		}

		public static long ClientTime
		{
			get
			{
				return DateTime.Now.Ticks / 10000;
			}
		}

		public static long ServerTime
		{
			get
			{
				return ClientTime + ServerTimeOffset;
			}
		}

		public static long ClientFrameStartTime
		{
			get; private set;
		}

		public static long ServerFrameStartTime
		{
			get
			{
				return ClientFrameStartTime + ServerTimeOffset;
			}
		}
		#endregion
	}
}

