using System.Text;

namespace GameFramework
{
	public static class ByteHelper
	{
		public static string ToHex(this byte b)
		{
			return b.ToString("X2");
		}

		public static string ToHex(this byte[] bytes)
		{
			StringBuilder stringBuilder = new();
			foreach (byte b in bytes)
			{
				stringBuilder.Append(b.ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		public static string ToHex(this byte[] bytes, string format)
		{
			StringBuilder stringBuilder = new();
			foreach (byte b in bytes)
			{
				stringBuilder.Append(b.ToString(format));
			}
			return stringBuilder.ToString();
		}

		public static string ToHex(this byte[] bytes, int offset, int count)
		{
			StringBuilder stringBuilder = new();
			for (int i = offset; i < offset + count; ++i)
			{
				stringBuilder.Append(bytes[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		public static string ToStr(this byte[] bytes)
		{
			return Encoding.Default.GetString(bytes);
		}

		public static string ToStr(this byte[] bytes, int index, int count)
		{
			return Encoding.Default.GetString(bytes, index, count);
		}

		public static string Utf8ToStr(this byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		public static string Utf8ToStr(this byte[] bytes, int index, int count)
		{
			return Encoding.UTF8.GetString(bytes, index, count);
		}

		public static void WriteTo(this byte[] bytes, int offset, uint value)
		{
			bytes[offset] = (byte)(value & 0xff);
			bytes[offset + 1] = (byte)((value & 0xff00) >> 8);
			bytes[offset + 2] = (byte)((value & 0xff0000) >> 16);
			bytes[offset + 3] = (byte)((value & 0xff000000) >> 24);
		}

		public static void WriteTo(this byte[] bytes, int offset, int value)
		{
			bytes[offset] = (byte)(value & 0xff);
			bytes[offset + 1] = (byte)((value & 0xff00) >> 8);
			bytes[offset + 2] = (byte)((value & 0xff0000) >> 16);
			bytes[offset + 3] = (byte)((value & 0xff000000) >> 24);
		}

		public static void WriteTo(this byte[] bytes, int offset, byte value)
		{
			bytes[offset] = value;
		}

		public static void WriteTo(this byte[] bytes, int offset, short value)
		{
			bytes[offset] = (byte)(value & 0xff);
			bytes[offset + 1] = (byte)((value & 0xff00) >> 8);
		}

		public static void WriteTo(this byte[] bytes, int offset, ushort value)
		{
			bytes[offset] = (byte)(value & 0xff);
			bytes[offset + 1] = (byte)((value & 0xff00) >> 8);
		}

		public static unsafe void WriteTo(this byte[] bytes, int offset, long value)
		{
			byte* bPoint = (byte*)&value;
			for (int i = 0; i < sizeof(long); ++i)
			{
				bytes[offset + i] = bPoint[i];
			}
		}

		public static long Hash(this byte[] data, int index, int length)
		{
			const int p = 16777619;
			long hash = 2166136261L;

			for (int i = index; i < index + length; i++)
			{
				hash = (hash ^ data[i]) * p;
			}

			hash += hash << 13;
			hash ^= hash >> 7;
			hash += hash << 3;
			hash ^= hash >> 17;
			hash += hash << 5;
			return hash;
		}
	}
}


