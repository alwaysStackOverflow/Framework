//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.IO;
using UnityEngine;

namespace UnityGameFramework
{
	/// <summary>
	/// 默认游戏框架日志辅助器。
	/// </summary>
	public class DefaultLogHelper : GameFrameworkLog.ILogHelper
	{
		/// <summary>
		/// 记录日志。
		/// </summary>
		/// <param name="level">日志等级。</param>
		/// <param name="message">日志内容。</param>
		public void Log(GameFrameworkLogLevel level, object message)
		{
#if UNITY_EDITOR
			switch (level)
			{
				case GameFrameworkLogLevel.Debug:
					Debug.Log($"<color=#888888>{message}</color>");
					break;

				case GameFrameworkLogLevel.Info:
					Debug.Log(message);
					break;

				case GameFrameworkLogLevel.Warning:
					Debug.LogWarning(message);
					break;

				case GameFrameworkLogLevel.Error:
					Debug.LogError(message);
					break;
				case GameFrameworkLogLevel.Fatal:
					Debug.LogError($"<color=#ff0000>{message}</color>");
					break;
			}
#else
			switch (level)
			{
#if ENABLE_LOG || ENABLE_DEBUG_LOG || ENABLE_DEBUG_AND_ABOVE_LOG
				case GameFrameworkLogLevel.Debug:
					WriteLog(message, level);
					break;
#endif

#if ENABLE_LOG || ENABLE_INFO_LOG || ENABLE_DEBUG_AND_ABOVE_LOG || ENABLE_INFO_AND_ABOVE_LOG
				case GameFrameworkLogLevel.Info:
					WriteLog(message, level);
					break;
#endif
#if ENABLE_LOG || ENABLE_WARNING_LOG || ENABLE_DEBUG_AND_ABOVE_LOG || ENABLE_INFO_AND_ABOVE_LOG || ENABLE_WARNING_AND_ABOVE_LOG
				case GameFrameworkLogLevel.Warning:
					WriteLog(message, level);
					break;
#endif
#if ENABLE_LOG || ENABLE_ERROR_LOG || ENABLE_DEBUG_AND_ABOVE_LOG || ENABLE_INFO_AND_ABOVE_LOG || ENABLE_WARNING_AND_ABOVE_LOG || ENABLE_ERROR_AND_ABOVE_LOG
				case GameFrameworkLogLevel.Error:
					WriteLog(message, level);
					break;
#endif
#if ENABLE_LOG || ENABLE_FATAL_LOG || ENABLE_DEBUG_AND_ABOVE_LOG || ENABLE_INFO_AND_ABOVE_LOG || ENABLE_WARNING_AND_ABOVE_LOG || ENABLE_ERROR_AND_ABOVE_LOG || ENABLE_FATAL_AND_ABOVE_LOG
				case GameFrameworkLogLevel.Fatal:
					WriteLog(message, level);
					break;
#endif
			}		
#endif
		}

		private string LogFilePath
		{
			get
			{
#if UNITY_EDITOR
				return Path.GetFullPath("Bundles/Savings");
#else
				return $"{Application.persistentDataPath}/Log";
#endif
			}
		}

		private void WriteLog(object message, GameFrameworkLogLevel level)
		{
			var log = $"{System.DateTime.Now:yyyy-MM-dd HH:MM:ss:ffff}: {level}: {message}";
			if (!System.IO.Directory.Exists(LogFilePath))
			{
				System.IO.Directory.CreateDirectory(LogFilePath);
			}
			var fileName = $"{LogFilePath}/{System.DateTime.Now:yyyy-MM-dd}.log";
			if (!System.IO.File.Exists(fileName))
			{
				System.IO.File.Create(fileName).Dispose();
			}
			using var writer = System.IO.File.AppendText(fileName);
			writer.WriteLine(log);
			writer.Flush();
			writer.Close();
		}
	}
}
