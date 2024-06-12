using GameFramework;
using GameFramework.Timer;
using UnityEngine;

namespace UnityGameFramework
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Game Framework/Timer")]
	public class TimerComponent : GameFrameworkComponent
	{
		private ITimerManager m_TimerManager = null;
		protected override void Awake()
		{
			base.Awake();

			m_TimerManager = GameFrameworkEntry.GetModule<ITimerManager>();
			if (m_TimerManager == null)
			{
				Log.Fatal("Timer manager is invalid.");
				return;
			}
		}

		public void Add(Timer timer)
		{
			m_TimerManager?.Add(timer);
		}

		public void Remove(Timer timer)
		{
			m_TimerManager?.Remove(timer);
		}

		public void Remove(string id)
		{
			m_TimerManager?.Remove(id);
		}
	}
}


