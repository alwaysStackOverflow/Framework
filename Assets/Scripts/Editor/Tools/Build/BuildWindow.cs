using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using UnityObject = UnityEngine.Object;


public class BuildWindow : OdinEditorWindow
{
	[MenuItem("��Ŀ����/Build����")]
	public static void ShowWindow()
	{
		var window = GetWindow<BuildWindow>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 610);
	}

	protected override void OnEnable()
	{

	}

	[Button("������ϷDll��bundle�ļ�����", ButtonSizes.Large)]
	private void CopyDllToBundle()
	{
		AssemblyTool.DoCompile();
	}
}
