#if UNITY_EDITOR
using System;
using System.IO;

namespace YooAsset
{
	public static class YooAssetPathUtility
	{
		/// <summary>
		/// 路径归一化
		/// 注意：替换为Linux路径格式
		/// </summary>
		public static string RegularPath(string path)
		{
			return PathUtility.RegularPath(path);
		}

		/// <summary>
		/// 移除路径里的后缀名
		/// </summary>
		public static string RemoveExtension(string str)
		{
			return PathUtility.RemoveExtension(str);
		}

		/// <summary>
		/// 合并路径
		/// </summary>
		public static string Combine(string path1, string path2)
		{
			return PathUtility.Combine(path1, path2);
		}

		/// <summary>
		/// 合并路径
		/// </summary>
		public static string Combine(string path1, string path2, string path3)
		{
			return PathUtility.Combine(path1, path2, path3);
		}

		/// <summary>
		/// 合并路径
		/// </summary>
		public static string Combine(string path1, string path2, string path3, string path4)
		{
			return PathUtility.Combine(path1, path2, path3, path4);
		}
	}

	/// <summary>
	/// 字符串工具类
	/// </summary>
	public static class YooAssetStringUtility
	{
		public static string Format(string format, object arg0)
		{
			return StringUtility.Format(format, arg0);
		}
		public static string Format(string format, object arg0, object arg1)
		{
			return StringUtility.Format(format, arg0, arg1);
		}
		public static string Format(string format, object arg0, object arg1, object arg2)
		{
			return StringUtility.Format(format, arg0, arg1, arg2);
		}
		public static string Format(string format, params object[] args)
		{
			return StringUtility.Format(format, args);
		}
	}

	/// <summary>
	/// 文件工具类
	/// </summary>
	public static class YooAssetFileUtility
	{
		/// <summary>
		/// 读取文件的文本数据
		/// </summary>
		public static string ReadAllText(string filePath)
		{
			return FileUtility.ReadAllText(filePath);
		}

		/// <summary>
		/// 读取文件的字节数据
		/// </summary>
		public static byte[] ReadAllBytes(string filePath)
		{
			return FileUtility.ReadAllBytes(filePath);
		}

		/// <summary>
		/// 写入文本数据（会覆盖指定路径的文件）
		/// </summary>
		public static void WriteAllText(string filePath, string content)
		{
			FileUtility.WriteAllText(filePath, content);
		}

		/// <summary>
		/// 写入字节数据（会覆盖指定路径的文件）
		/// </summary>
		public static void WriteAllBytes(string filePath, byte[] data)
		{
			FileUtility.WriteAllBytes(filePath, data);
		}

		/// <summary>
		/// 创建文件的文件夹路径
		/// </summary>
		public static void CreateFileDirectory(string filePath)
		{
			FileUtility.CreateFileDirectory(filePath);
		}

		/// <summary>
		/// 创建文件夹路径
		/// </summary>
		public static void CreateDirectory(string directory)
		{
			FileUtility.CreateDirectory(directory);
		}

		/// <summary>
		/// 获取文件大小（字节数）
		/// </summary>
		public static long GetFileSize(string filePath)
		{
			return FileUtility.GetFileSize(filePath);
		}
	}

	/// <summary>
	/// 哈希工具类
	/// </summary>
	public static class YooAssetHashUtility
	{
		public static string ToString(byte[] hashBytes)
		{
			string result = BitConverter.ToString(hashBytes);
			result = result.Replace("-", "");
			return result.ToLower();
		}

		#region SHA1
		/// <summary>
		/// 获取字符串的Hash值
		/// </summary>
		public static string StringSHA1(string str)
		{
			return HashUtility.StringSHA1(str);
		}

		/// <summary>
		/// 获取文件的Hash值
		/// </summary>
		public static string FileSHA1(string filePath)
		{
			return HashUtility.FileSHA1(filePath);
		}

		/// <summary>
		/// 获取数据流的Hash值
		/// </summary>
		public static string StreamSHA1(Stream stream)
		{
			return HashUtility.StreamSHA1(stream);
		}

		/// <summary>
		/// 获取字节数组的Hash值
		/// </summary>
		public static string BytesSHA1(byte[] buffer)
		{
			return HashUtility.BytesSHA1(buffer);
		}
		#endregion

		#region MD5
		/// <summary>
		/// 获取字符串的MD5
		/// </summary>
		public static string StringMD5(string str)
		{
			return HashUtility.StringMD5(str);
		}

		/// <summary>
		/// 获取文件的MD5
		/// </summary>
		public static string FileMD5(string filePath)
		{
			return HashUtility.FileMD5(filePath);
		}

		/// <summary>
		/// 获取数据流的MD5
		/// </summary>
		public static string StreamMD5(Stream stream)
		{
			return HashUtility.StreamMD5(stream);
		}

		/// <summary>
		/// 获取字节数组的MD5
		/// </summary>
		public static string BytesMD5(byte[] buffer)
		{
			return HashUtility.BytesMD5(buffer);
		}
		#endregion

		#region CRC32
		/// <summary>
		/// 获取字符串的CRC32
		/// </summary>
		public static string StringCRC32(string str)
		{
			return HashUtility.StringCRC32(str);
		}

		/// <summary>
		/// 获取文件的CRC32
		/// </summary>
		public static string FileCRC32(string filePath)
		{
			return HashUtility.FileCRC32(filePath);
		}

		/// <summary>
		/// 获取数据流的CRC32
		/// </summary>
		public static string StreamCRC32(Stream stream)
		{
			return HashUtility.StreamCRC32(stream);
		}

		/// <summary>
		/// 获取字节数组的CRC32
		/// </summary>
		public static string BytesCRC32(byte[] buffer)
		{
			return HashUtility.BytesCRC32(buffer);
		}
		#endregion
	}
}
#endif