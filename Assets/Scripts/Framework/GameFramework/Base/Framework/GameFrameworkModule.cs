//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 游戏框架模块抽象类。
    /// </summary>
    internal abstract class GameFrameworkModule
    {
		public const int GameFrameworkModuleDefaultPriority = -999;
		public const int TimerManagerPriority = 8;
		public const int NetworkManagerPriority = 7;
		public const int EventManagerPriority = 6;
		public const int SingletonManagerPriority = 5;
		public const int UIManagerPriority = 4;
		public const int ObjectPoolManagerPriority = 3;
		public const int DownloadManagerPriority = 2;
        public const int FileSystemManagerPriority = 1;
		public const int FsmManagerPriority = 0;
		public const int ProcedureManagerPriority = -1;
		public const int ResourceManagerPriority = -2;
		public const int SceneManagerPriority = -3;
		public const int DebuggerManagerPriority = -4;

		/// <summary>
		/// 获取游戏框架模块优先级。
		/// </summary>
		/// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
		internal virtual int Priority => GameFrameworkModuleDefaultPriority;
		internal virtual void FixedUpdate() { }

		internal virtual void LateUpdate() { }
		/// <summary>
		/// 游戏框架模块轮询。
		/// </summary>
		/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
		/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
		internal abstract void Update(float elapseSeconds, float realElapseSeconds);

		/// <summary>
		/// 关闭并清理游戏框架模块。
		/// </summary>
		internal abstract void Shutdown();
    }
}
