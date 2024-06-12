using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Image), true)]
[CanEditMultipleObjects]
public class ImageEditorEx : ImageEditor
{
	private Image _image;
	private BoxCollider2D _boxCollider;
	private RectTransform _rectTransform;
	protected override void OnEnable()
	{
		base.OnEnable();
		_image = (Image)target;
		_boxCollider = _image.GetComponent<BoxCollider2D>();
		_rectTransform = _image.GetComponent<RectTransform>();
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();
		if (_boxCollider != null)
		{
			if (GUILayout.Button("BoxCollider2D FitSize"))
			{
				serializedObject.Update();
				_boxCollider.size = _rectTransform.rect.size;
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}


[CustomEditor(typeof(RawImage), true)]
[CanEditMultipleObjects]
public class RawImageEditorEx : RawImageEditor
{
	private RawImage _image;
	private BoxCollider2D _boxCollider;
	private RectTransform _rectTransform;
	protected override void OnEnable()
	{
		base.OnEnable();
		_image = (RawImage)target;
		_boxCollider = _image.GetComponent<BoxCollider2D>();
		_rectTransform = _image.GetComponent<RectTransform>();
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();

		if (_boxCollider != null)
		{
			if (GUILayout.Button("BoxCollider2D FitSize"))
			{
				serializedObject.Update();
				_boxCollider.size = _rectTransform.rect.size;
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();
			}
		}
	}
}