using UnityEngine;

namespace Client
{
	public enum UILayer
	{
		LowUI,
		MidUI,
		HighUI,
		Entity,
		_3D,
	}

	public class GameCamera : MonoBehaviour
	{
		public static GameCamera Instance { get; private set; }

		[SerializeField]
		private Camera _3dCamera;
		[SerializeField]
		private Camera _lowUICamera;
		[SerializeField]
		private Camera _midUICamera;
		[SerializeField]
		private Camera _highUICamera;
		[SerializeField]
		private Camera _entityUICamera;
		[SerializeField]
		private GameObject _root;
		[SerializeField]
		private GameObject _light;

		public Camera Camera3D => _3dCamera;
		public Camera LowUICamera => _lowUICamera;
		public Camera MidUICamera => _midUICamera;
		public Camera HighUICamera => _highUICamera;
		public Camera EntityUICamera => _entityUICamera;
		public GameObject Light => _light;
		public GameObject Root => _root;

		public Camera GetCamera(UILayer level)
		{
			switch (level)
			{
				case UILayer.LowUI:
					return LowUICamera;
				case UILayer.MidUI:
					return MidUICamera;
				case UILayer.HighUI:
					return HighUICamera;
				case UILayer.Entity:
					return EntityUICamera;
				default:
					return Camera3D;
			}
		}

		public static LayerMask GetLayer(UILayer level)
		{
			switch (level)
			{
				case UILayer.LowUI:
					return LayerMask.NameToLayer("LowUI");
				case UILayer.MidUI:
					return LayerMask.NameToLayer("MidUI");
				case UILayer.HighUI:
					return LayerMask.NameToLayer("HighUI");
				case UILayer.Entity:
					return LayerMask.NameToLayer("Entity");
				default:
					return LayerMask.NameToLayer("Default");
			}
		}

		public void Awake()
		{
			Instance = this;
		}

		public void OnDestroy()
		{
			Instance = null;
		}
	}
}
