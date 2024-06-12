using Client;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(UIImage), true)]
[CanEditMultipleObjects]
public class UIImageInspector : ImageEditorEx
{
	private UIImage _UIimage;
	private SpriteAtlas _atlas;
	private SerializedProperty _atlasProperity = null;
	private SerializedProperty _selectSprite = null;
	private SerializedProperty _currentSpriteName = null;

	private List<Sprite> _atlasSprites;
	private string[] _atlasSpriteNames;
	private int _selectIndex;

	protected override void OnEnable()
	{
		base.OnEnable();
		_UIimage = (UIImage)target;
		_atlasProperity = serializedObject.FindProperty("_atlas");
		_currentSpriteName = serializedObject.FindProperty("_currentSpriteName");
	}

	private bool init;
	private void Init()
	{
		_atlasSprites = _UIimage.GetSprites();
		_atlasSpriteNames = new string[_atlasSprites.Count];
		var index = 0;
		for (int i = 0; i < _atlasSprites.Count; i++)
		{
			_atlasSpriteNames[i] = _atlasSprites[i].name.Replace("(Clone)", "");
			if(_atlasSpriteNames[i] == _currentSpriteName.stringValue)
			{
				index = i;
			}
		}
		_selectIndex = index;
		_selectSprite = serializedObject.FindProperty("m_Sprite");
		init = true;
	}

	public override void OnInspectorGUI()
	{

		serializedObject.Update();
		_atlas = (SpriteAtlas)EditorGUILayout.ObjectField("Atlas", _UIimage.Atlas, typeof(SpriteAtlas), false);
		if (_atlas != null)
		{
			_atlasProperity.objectReferenceValue = _atlas;
		}
		if (_UIimage.Atlas != null)
		{
			if (!init)
			{
				Init();
			}
			_selectIndex = EditorGUILayout.Popup("Sprite", _selectIndex, _atlasSpriteNames);
			//_selectSprite.objectReferenceValue = _atlasSprites[_selectIndex];
			_currentSpriteName.stringValue = _atlasSpriteNames[_selectIndex];
		}
		serializedObject.ApplyModifiedProperties();
		base.OnInspectorGUI();
	}
}
