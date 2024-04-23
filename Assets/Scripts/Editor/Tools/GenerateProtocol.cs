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


public class GenerateProtocol : OdinEditorWindow
{
	[MenuItem("项目工具/协议文件工具")]
	public static void ShowWindow()
	{
		var window = GetWindow<GenerateProtocol>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 610);
	}

	protected override void OnEnable()
	{
		
	}

	[PropertyOrder(999999),TextArea(30, 40)]
	public string log;
	private readonly Encoding GBK = Encoding.GetEncoding("GBK");

	private void WriteLog(object logObj)
	{
		log = $"{log}\n{logObj}";
	}

	[Button("生成CS文件", ButtonSizes.Large)]
	public void GenerateCSharpFile()
    {
		File.Copy("./Assets/Scripts/Protocol/Proto/Protocol.proto", "./Data_Editor/Protobuf/Protocol.proto", true);

		System.Diagnostics.Process cmd = new System.Diagnostics.Process();
		cmd.StartInfo.FileName = "cmd.exe";
		cmd.StartInfo.UseShellExecute = false;          //是否使用操作系统shell启动
		cmd.StartInfo.RedirectStandardInput = true;     //接受来自调用程序的输入信息
		cmd.StartInfo.RedirectStandardOutput = true;    //由调用程序获取输出信息
		cmd.StartInfo.RedirectStandardError = true;     //重定向标准错误输出
		cmd.StartInfo.CreateNoWindow = true;            //不显示程序窗口
		cmd.StartInfo.StandardOutputEncoding = GBK;
		cmd.StartInfo.StandardErrorEncoding = GBK;
		cmd.Start();

		StreamWriter utf8Writer = new(cmd.StandardInput.BaseStream, GBK);
		utf8Writer.WriteLine($"cd ./Data_Editor/Protobuf");
		utf8Writer.WriteLine(".\\Generate.bat");
		utf8Writer.Flush();
		utf8Writer.Close();
		WriteLog(cmd.StandardOutput.ReadToEnd());
		cmd.WaitForExit();

		File.Copy("./Data_Editor/Protobuf/Protocol.cs", "./Assets/Scripts/Protocol/Class/ProtocolClass.cs", true);
		File.Delete("./Data_Editor/Protobuf/Protocol.proto");
		File.Delete("./Data_Editor/Protobuf/Protocol.cs");
		AssetDatabase.Refresh();
		WriteLog("生成成功");
	}
}
