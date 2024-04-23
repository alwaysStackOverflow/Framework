using UnityEditor;
using System.IO;
using HybridCLR.Editor.Settings;
using UnityGameFramework;

public static class HybirdClrTools
{
	[MenuItem("��Ŀ����/����AotDll��bundle�ļ�����")]
	public static void CopyAotDll()
	{
		BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
		string fromDir = Path.Combine(HybridCLRSettings.Instance.strippedAOTDllOutputRootDir, target.ToString());
		string toDir = "Assets/Bundles/Bin/AotDll";
		if (Directory.Exists(toDir))
		{
			Directory.Delete(toDir, true);
		}
		Directory.CreateDirectory(toDir);

		foreach (string aotDll in HybridCLRSettings.Instance.patchAOTAssemblies)
		{
			File.Copy(Path.Combine(fromDir, aotDll), Path.Combine(toDir, $"{aotDll}.bytes"), true);
		}
		Log.Info($"CopyAotDll Finish!");

		AssetDatabase.Refresh();
	}
}
