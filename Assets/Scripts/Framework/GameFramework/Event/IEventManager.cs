//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Network;

namespace GameFramework.Event
{
    /// <summary>
    /// 事件管理器接口。
    /// </summary>
    public interface IEventManager
    {
		#region Event
		public void Subscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs;

        public void Unsubscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs;

        public void Fire<T>(T e = default) where T : BaseEventArgs;

        public void FireNow<T>(T e = default) where T : BaseEventArgs;
		#endregion Event

		#region Network
		public void Listen<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject;
		public void Unlisten<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject;

        public void Trigger<T>(T e) where T : ProtoObject;

        public void TriggerNow<T>(T e) where T : ProtoObject;
		#endregion Network
	}
}
