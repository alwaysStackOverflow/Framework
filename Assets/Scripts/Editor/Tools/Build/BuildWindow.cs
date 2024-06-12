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
	[MenuItem("项目工具/Build工具")]
	public static void ShowWindow()
	{
		var window = GetWindow<BuildWindow>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 610);
	}

	protected override void OnEnable()
	{

	}

	[Button("复制游戏Dll到bundle文件夹下", ButtonSizes.Large)]
	private void CopyDllToBundle()
	{
		AssemblyTool.DoCompile();
	}
}
