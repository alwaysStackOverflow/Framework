using UnityEngine;

namespace Client
{
	[AddComponentMenu("UI/BoxCollider2DSizeUpdater")]
	[RequireComponent(typeof(BoxCollider2D), typeof(RectTransform))]
	public class BoxCollider2DSizeUpdater : MonoBehaviour
	{
		public bool BoxCollider2DAutoFitSize;

		private RectTransform _transform;
		private BoxCollider2D _collider;

		private void Start()
		{
			_transform = gameObject.GetOrAddComponent<RectTransform>();
			_collider = gameObject.GetOrAddComponent<BoxCollider2D>();
			FitBoxCollider2DSize();
		}

		private void Update()
		{
			if (BoxCollider2DAutoFitSize)
			{
				FitBoxCollider2DSize();
			}
		}

		public void FitBoxCollider2DSize()
		{
			_collider.size = _transform.rect.size;
		}
	}
}
