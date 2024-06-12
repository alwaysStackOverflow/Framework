using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ClearSavingsWindow : OdinEditorWindow
{
	[MenuItem("项目工具/游戏记录窗口")]
	public static void ShowWindow()
	{
		var window = GetWindow<ClearSavingsWindow>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 610);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Savings = new();
		var list = new List<FileInfo>();
		GetSavingsFiles(ref list);
		foreach (var file in list)
		{
			Savings.Add(file.FullName);
		}
	}
	public string GameDatabaseDirectory
	{
		get
		{
#if UNITY_EDITOR
			var path = Path.GetFullPath("Bundles/Savings");
#else
				var path = $"{UnityEngine.Application.persistentDataPath}/Savings";
#endif
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return path;
		}
	}
	public List<string> GetSavingsPath()
	{
		if (!Directory.Exists(GameDatabaseDirectory))
		{
			Directory.CreateDirectory(GameDatabaseDirectory);
			return new List<string>();
		}
		var allFile = Directory.GetFiles(GameDatabaseDirectory);
		var result = new List<string>();
		foreach (var file in allFile)
		{
			if (Path.GetExtension(file).ToLower() == ".db")
			{
				result.Add(file);
			}
		}
		return result;
	}

	public FileInfo GetSavingsFileInfo(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return null;
		}
		return new FileInfo(fileName);
	}

	private void GetSavingsFiles(ref List<FileInfo> list)
	{
		var savings = GetSavingsPath();
		foreach (var file in savings)
		{
			list.Add(GetSavingsFileInfo(file));
		}
	}

	[ListDrawerSettings(HideAddButton = true, CustomRemoveIndexFunction = "CustomRemoveIndexFunction")]
	public List<string> Savings;

	public void CustomRemoveIndexFunction(int index)
	{
		File.Delete(Savings[index]);
		Savings.RemoveAt(index);
	}
}
