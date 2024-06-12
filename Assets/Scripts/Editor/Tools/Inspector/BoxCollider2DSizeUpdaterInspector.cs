using Client;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

[CustomEditor(typeof(BoxCollider2DSizeUpdater), true)]
[CanEditMultipleObjects]
public class BoxCollider2DSizeUpdaterInspector : GameFrameworkInspector
{
	private BoxCollider2DSizeUpdater _updater;
	private BoxCollider2D _boxCollider2d;
	private RectTransform _rectTransform;
	private SerializedProperty _autoFitProperity = null;
	private bool _isAutoFit;
	private void OnEnable()
	{
		_autoFitProperity = serializedObject.FindProperty("BoxCollider2DAutoFitSize");
		_updater = (BoxCollider2DSizeUpdater)target;
		_boxCollider2d = _updater.gameObject.GetOrAddComponent<BoxCollider2D>();
		_rectTransform = _updater.gameObject.GetOrAddComponent<RectTransform>();
		_isAutoFit = false;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();

		_isAutoFit = EditorGUILayout.Toggle("BoxCollider2D Auto Fit Size", _autoFitProperity.boolValue);
		_autoFitProperity.boolValue = _isAutoFit;
		if (_isAutoFit)
		{
			_boxCollider2d.size = _rectTransform.rect.size;
		}
		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("BoxCollider2D FitSize"))
		{
			serializedObject.Update();
			_boxCollider2d.size = _rectTransform.rect.size;
			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();
		}
	}
}