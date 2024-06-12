using System;
using System.Collections.Generic;

namespace GameFramework
{
	public static partial class Utility
	{
		public static class Math
		{
			public const int OneK = 1024;
			public const int OneM = 1024 * 1024;

			public static long Floor(double value)
			{
				var v = (decimal)value;
				return (long)System.Math.Floor(v);
			}

			public static long Ceiling(double value)
			{
				var v = (decimal)value;
				return (long)System.Math.Ceiling(v);
			}

			public static long Floor(decimal value)
			{
				return (long)System.Math.Floor(value);
			}

			public static long Ceiling(decimal value)
			{
				return (long)System.Math.Ceiling(value);
			}
		}
	}

}


