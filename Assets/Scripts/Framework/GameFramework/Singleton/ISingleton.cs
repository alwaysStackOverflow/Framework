
namespace GameFramework.Singleton
{
	internal interface ISingleton
	{
		public void Register();
		public void Init();
		public void Release();
		public void Update(float elapseSeconds, float realElapseSeconds);
	}

	public abstract class Singleton<T> : ISingleton where T : class
	{
		public abstract int Priority { get; }
		public static T Instance{ get; private set; }

		public void Register()
		{
			Instance = this as T;
		}

		public abstract void Init();
		public abstract void Update(float elapseSeconds, float realElapseSeconds);
		public abstract void Release();
	}
}