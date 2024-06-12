using GameFramework;
using GameFramework.Singleton;
using UnityEngine;

namespace UnityGameFramework
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Game Framework/Singleton")]
	public class SingletonComponent : GameFrameworkComponent
	{
		private ISingletonManager m_SingletonManager = null;
		protected override void Awake()
		{
			base.Awake();

			m_SingletonManager = GameFrameworkEntry.GetModule<ISingletonManager>();
			if (m_SingletonManager == null)
			{
				Log.Fatal("Singleton manager is invalid.");
				return;
			}
		}

		public void AddSingleton<T>() where T : Singleton<T>, new()
		{
			m_SingletonManager?.AddSingleton<T>();
		}
	}
}


