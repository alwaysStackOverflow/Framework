//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using System;
using UnityEngine;

namespace UnityGameFramework
{
    /// <summary>
    /// 事件组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Event")]
    public sealed class EventComponent : GameFrameworkComponent
    {
        private IEventManager m_EventManager = null;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_EventManager = GameFrameworkEntry.GetModule<IEventManager>();
            if (m_EventManager == null)
            {
                Log.Fatal("Event manager is invalid.");
                return;
            }
        }

        private void Start()
        {
        }

		#region Event
		/// <summary>
		/// 订阅事件处理回调函数。
		/// </summary>
		/// <param name="id">事件类型编号。</param>
		/// <param name="handler">要订阅的事件处理回调函数。</param>
		public void Subscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
            m_EventManager.Subscribe(id, handler);
        }

		/// <summary>
		/// 取消订阅事件处理回调函数。
		/// </summary>
		/// <param name="id">事件类型编号。</param>
		/// <param name="handler">要取消订阅的事件处理回调函数。</param>
		public void Unsubscribe<T>(string id, GameFrameworkAction<T> handler) where T : BaseEventArgs
		{
            m_EventManager.Unsubscribe(id, handler);
        }


        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="e">事件内容。</param>
        public void Fire<T>(T e = default) where T : BaseEventArgs
		{
            m_EventManager.Fire(e);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="e">事件内容。</param>
        public void FireNow<T>(T e = default) where T : BaseEventArgs
		{
            m_EventManager.FireNow(e);
        }
		#endregion Event

		#region Network
		/// <summary>
		/// 订阅网络消息。
		/// </summary>
		/// <param name="id">协议ID</param>
		/// <param name="handler">网络消息回调函数。</param>
		public void Listen<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			m_EventManager.Listen(id, handler);
		}

		/// <summary>
		/// 取消订阅网络消息。
		/// </summary>
		/// <param name="id">协议ID</param>
		/// <param name="handler">网络消息回调函数。</param>
		public void Unlisten<T>(int id, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			m_EventManager.Unlisten(id, handler);
		}

		/// <summary>
		/// 抛出网络消息，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
		/// </summary>
		/// <param name="e">协议对象。</param>
		public void Trigger<T>(T e) where T : ProtoObject
		{
            m_EventManager.Trigger(e);
		}

		/// <summary>
		/// 抛出网络消息，这个操作不是线程安全的，事件会立刻分发。
		/// </summary>
		/// <param name="e">协议对象。</param>
		public void TriggerNow<T>(T e) where T : ProtoObject
		{
			m_EventManager.TriggerNow(e);
		}
		#endregion Network
	}
}
