//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
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
				default:
					Debug.Log(message);
					break;
			}
#else
			WriteLog(message);
#endif
		}

		private readonly string LogFilePath = $"{Application.persistentDataPath}/Log";
        private void WriteLog(object message)
        {
			var log = $"{System.DateTime.Now:yyyy-MM-dd HH:MM:ss:ffff}: {message}";
            if(!System.IO.Directory.Exists(LogFilePath))
            {
				System.IO.Directory.CreateDirectory(LogFilePath);
			}
            var fileName = $"{LogFilePath}/{System.DateTime.Now:yyyy-MM-dd}.log";
            if(!System.IO.File.Exists(fileName))
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
