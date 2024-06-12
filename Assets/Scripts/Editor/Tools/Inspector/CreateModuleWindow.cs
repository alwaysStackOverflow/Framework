using System;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Client;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;
using Common;
using UnityGameFramework;

#pragma warning disable CS0162
public class CreateModuleWindow : OdinEditorWindow
{
	[MenuItem("项目工具/创建模块工具")]
	public static void ShowWindow()
	{
		var window = GetWindow<CreateModuleWindow>();
		window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 1000);
	}

	protected override void OnEnable()
	{
		_moduleName = "";
		_optionType = OptionType.CreateModule;
		_views.Clear();
	}

	private readonly string ModuleParentPath = $"{Path.GetFullPath("Assets/Scripts/Client/GameLogic")}";
	private readonly string ModuleTypeScriptPath = $"{Path.GetFullPath("Assets/Scripts/Client/Base/MVC/ModuleType.cs")}";
	private readonly string ModuleManagerScriptPath = $"{Path.GetFullPath("Assets/Scripts/Client/Base/MVC/ModuleManager.cs")}";

	private enum OptionType
	{
		CreateModule,
		AddView,
	}

	[SerializeField]
	private OptionType _optionType;

	#region CreateModule

	[ShowIf("@_optionType == OptionType.CreateModule")]
	[SerializeField]
	private string _moduleName;

	[ShowIf("@_optionType == OptionType.CreateModule")]
	[SerializeField]
	private bool _isAwake;

	[ShowIf("@_optionType == OptionType.CreateModule")]
	[Button("创建模块", ButtonSizes.Large)]
	private void CreateModule()
	{
		if (string.IsNullOrWhiteSpace(_moduleName))
		{
			Log.Error("请输入正确的模块名称");
			return;
		}
		var moduleFolderPath = $"{ModuleParentPath}/{_moduleName}";
		var modulePath = $"{moduleFolderPath}/{_moduleName}Module.cs";
		var controllerPath = $"{moduleFolderPath}/{_moduleName}Controller.cs";
		var dataPath = $"{moduleFolderPath}/{_moduleName}Data.cs";
		var eventPath = $"{moduleFolderPath}/{_moduleName}Event.cs";

		var lines = new List<string>(File.ReadAllLines(ModuleTypeScriptPath));
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("public enum ModuleType"))
			{
				for (int j = i + 1; j < lines.Count; j++)
				{
					if (lines[j].Contains("}"))
					{
						if (!lines[j - 1].Contains(","))
						{
							lines[j - 1] = $"\t\t{lines[j - 1].Trim()},\r\n";
						}
						lines.Insert(j, $"\t\t{_moduleName},");
						break;
					}
				}
				break;
			}
		}
		File.WriteAllLines(ModuleTypeScriptPath, lines.ToArray());

		lines = new List<string>(File.ReadAllLines(ModuleManagerScriptPath));
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("public override void Init()"))
			{
				for (int j = i + 1; j < lines.Count; j++)
				{
					if (lines[j].Contains("}"))
					{
						lines.Insert(j, $"\t\t\tRegisterModule<{_moduleName}Module>({(_isAwake ? "true" : "false")});");
						break;
					}
				}
				break;
			}
		}
		File.WriteAllLines(ModuleManagerScriptPath, lines.ToArray());

		if (!Directory.Exists(moduleFolderPath))
		{
			Directory.CreateDirectory(moduleFolderPath);
		}
		if (!File.Exists(modulePath))
		{
			File.Create(modulePath).Close();
			File.WriteAllText(modulePath,
				$"namespace Client\r\n" +
				$"{{\r\n" +
					$"\tpublic class {_moduleName}Module : AModule<{_moduleName}Data,{_moduleName}Controller>\r\n" +
					$"\t{{\r\n" +
						$"\t\tpublic override ModuleType ModuleType => ModuleType.{_moduleName};\r\n\r\n" +
						$"\t\tpublic override void RegisterViewConfig()\r\n" +
						$"\t\t{{\r\n" +
						$"\t\t}}\r\n" +
					$"\t}}\r\n" +
				$"}}\r\n"
			);
		}
		if (!File.Exists(controllerPath))
		{
			File.Create(controllerPath).Close();
			File.WriteAllText(controllerPath,
				$"namespace Client\r\n" +
				$"{{\r\n" +
					$"\tpublic class {_moduleName}Controller : AController\r\n" +
					$"\t{{\r\n" +
						$"\t\tpublic {_moduleName}Data Data {{ get; private set; }}\r\n\r\n" +
						$"\t\tprotected override void OnInit()\r\n" +
						$"\t\t{{\r\n" +
							$"\t\t\tData = (Module as {_moduleName}Module).Model;\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tprotected override void OnAwake()\r\n" +
						$"\t\t{{\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tprotected override void OnShutdown()\r\n" +
						$"\t\t{{\r\n" +
						$"\t\t}}\r\n" +
					$"\t}}\r\n" +
				$"}}\r\n");
		}
		if (!File.Exists(dataPath))
		{
			File.Create(dataPath).Close();
			File.WriteAllText(dataPath,
				$"namespace Client\r\n" +
				$"{{\r\n" +
					$"\tpublic class {_moduleName}Data : AModel\r\n" +
					$"\t{{\r\n" +
						$"\t\tpublic override void ClearData()\r\n" +
						$"\t\t{{\r\n" +
						$"\t\t}}\r\n" +
					$"\t}}\r\n" +
				$"}}\r\n");
		}
		if (!File.Exists(eventPath))
		{
			File.Create(eventPath).Close();
			File.WriteAllText(eventPath,
				$"using GameFramework;\r\n\r\n" +
				$"namespace Client\r\n" +
				$"{{\r\n" +
				$"}}\r\n");
		}
		EditorUtility.DisplayDialog("创建模块", $"创建 {_moduleName}Module成功", "确认");
		AssetDatabase.Refresh();
	}
	#endregion CreateModule

	#region AddView

	[Serializable]
	private class ViewInfo
	{
		public string ViewName;
		public bool IncluedDestroyEvent;
	}

	[ShowIf("@_optionType == OptionType.AddView")]
	[FolderPath(ParentFolder = "Assets/Scripts/Client/GameLogic")]
	[SerializeField]
	private string _modifyModulePath;

	[ShowIf("@_optionType == OptionType.AddView")]
	[SerializeField]
	private List<ViewInfo> _views = new();

	[ShowIf("@_optionType == OptionType.AddView")]
	[ListDrawerSettings(NumberOfItemsPerPage = 5, Expanded = true, ShowPaging = true)]
	[Button("添加View", ButtonSizes.Large)]
	private void AddView()
	{
		if (string.IsNullOrWhiteSpace(_modifyModulePath))
		{
			Log.Error("请输入正确的模块路径");
		}
		foreach (var view in _views)
		{
			AddSingleView(view);
		}
	}

	private void AddSingleView(ViewInfo info)
	{
		if (string.IsNullOrWhiteSpace(info.ViewName))
		{
			return;
		}
		var eventPath = $"{ModuleParentPath}/{_modifyModulePath}/{_modifyModulePath}Event.cs";
		var controllerPath = $"{ModuleParentPath}/{_modifyModulePath}/{_modifyModulePath}Controller.cs";
		var viewPath = $"{ModuleParentPath}/{_modifyModulePath}/{info.ViewName}.cs";
		var UITypePath = $"{Path.GetFullPath("Assets/Scripts/Client/Config/ResourceConfig.cs")}";
		var modulePath = $"{ModuleParentPath}/{_modifyModulePath}/{_modifyModulePath}Module.cs";
		if (!File.Exists(viewPath))
		{
			File.Create(viewPath).Close();
			File.WriteAllText(viewPath,
				$"namespace Client\r\n" +
				$"{{\r\n" +
					$"\tpublic class {info.ViewName} : UIBaseForm\r\n" +
					$"\t{{\r\n" +
						$"\t\tpublic {_modifyModulePath}Data Data {{ get; private set; }}\r\n" +
						$"\t\tpublic {info.ViewName}() : base(UIType.{info.ViewName})\r\n" +
						$"\t\t{{\r\n" +
							$"\t\t\tData = ModuleManager.Instance.GetModule<{_modifyModulePath}Module>(ModuleType.{_modifyModulePath}).Model;\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tprotected override void OnInit()\r\n" +
						$"\t\t{{\r\n" +
							$"\t\t\tInitUnityObject();\r\n" +
							$"\t\t\tInitEvent();\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tpublic void InitUnityObject()\r\n" +
						$"\t\t{{\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tpublic void InitEvent()\r\n" +
						$"\t\t{{\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tprotected override void OnDestroy()\r\n" +
						$"\t\t{{\r\n\r\n" +
						$"\t\t}}\r\n" +
					$"\t}}\r\n" +
				$"}}\r\n");
		}

		var lines = new List<string>(File.ReadAllLines(UITypePath));
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("public enum UIType"))
			{
				for (int j = i + 1; j < lines.Count; j++)
				{
					if (lines[j].Contains("}"))
					{
						if (!lines[j - 1].Contains(","))
						{
							lines[j - 1] = $"\t\t{lines[j - 1].Trim()},\r\n";
						}
						lines.Insert(j, $"\t\t{info.ViewName},");
						break;
					}
				}
				break;
			}
		}
		File.WriteAllLines(UITypePath, lines.ToArray());

		lines = new List<string>(File.ReadAllLines(eventPath));
		for (int i = lines.Count - 1; i >= 0; i--)
		{
			if (lines[i].StartsWith("}"))
			{
				lines.Insert(i,
						$"\r\n" +
						$"\tpublic class Open{info.ViewName}Event : GameEventArgs\r\n" +
						$"\t{{\r\n" +
							$"\t\tpublic const string EventId = \"Client.Open{info.ViewName}Event\";\r\n" +
							$"\t\tpublic override string Id => EventId;\r\n\r\n" +
							$"\t\tpublic override void Clear()\r\n" +
							$"\t\t{{\r\n\r\n" +
							$"\t\t}}\r\n\r\n" +
							$"\t\tpublic static void Fire()\r\n" +
							$"\t\t{{\r\n" +
								$"\t\t\tReferencePool.Acquire<Open{info.ViewName}Event>().Invoke();\r\n" +
							$"\t\t}}\r\n" +
						$"\t}}\r\n\r\n");
				lines.Insert(i + 1,
					$"\tpublic class OnClose{info.ViewName}Event : GameEventArgs\r\n" +
					$"\t{{\r\n" +
						$"\t\tpublic const string EventId = \"Client.OnClose{info.ViewName}Event\";\r\n" +
						$"\t\tpublic override string Id => EventId;\r\n\r\n" +
						$"\t\tpublic override void Clear()\r\n" +
						$"\t\t{{\r\n\r\n" +
						$"\t\t}}\r\n\r\n" +
						$"\t\tpublic static OnClose{info.ViewName}Event Create()\r\n" +
						$"\t\t{{\r\n" +
							$"\t\t\treturn ReferencePool.Acquire<OnClose{info.ViewName}Event>();\r\n" +
						$"\t\t}}\r\n" +
					$"\t}}");
				if (info.IncluedDestroyEvent)
				{
					lines.Insert(i + 1,
						$"\tpublic class Destroy{info.ViewName}Event : GameEventArgs\r\n" +
						$"\t{{\r\n" +
							$"\t\tpublic const string EventId = \"Client.Destroy{info.ViewName}Event\";\r\n" +
							$"\t\tpublic override string Id => EventId;\r\n\r\n" +
							$"\t\tpublic override void Clear()\r\n" +
							$"\t\t{{\r\n\r\n" +
							$"\t\t}}\r\n\r\n" +
							$"\t\tpublic static void Fire()\r\n" +
							$"\t\t{{\r\n" +
								$"\t\t\tReferencePool.Acquire<Destroy{info.ViewName}Event>().Invoke();\r\n" +
							$"\t\t}}\r\n" +
						$"\t}}\r\n\r\n");
				}
				break;
			}
		}
		File.WriteAllLines(eventPath, lines.ToArray());


		lines = new List<string>(File.ReadAllLines(controllerPath));
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("protected override void OnInit()"))
			{
				lines.Insert(i - 1, $"\t\tprivate {info.ViewName} m_{info.ViewName}View;");
				break;
			}
		}
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("protected override void OnAwake()"))
			{
				for (int j = i + 1; j < lines.Count; j++)
				{
					if (lines[j].Contains("}"))
					{
						lines.Insert(j,
							$"\t\t\tSubscribe<Open{info.ViewName}Event>(Open{info.ViewName}Event.EventId, OnOpen{info.ViewName});\r\n" +
							(info.IncluedDestroyEvent ? $"\t\t\tSubscribe<Destroy{info.ViewName}Event>(Destroy{info.ViewName}Event.EventId, OnDestroy{info.ViewName});\r\n" : "") +
							$"\t\t\tSubscribe<OnClose{info.ViewName}Event>(OnClose{info.ViewName}Event.EventId, OnClose{info.ViewName});");
						break;
					}
				}
				break;
			}
		}

		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("protected override void OnShutdown()"))
			{
				for (int j = i + 1; j < lines.Count; j++)
				{
					if (lines[j].Contains("}"))
					{
						lines.Insert(j,
							$"\t\t\tUnsubscribe<Open{info.ViewName}Event>(Open{info.ViewName}Event.EventId, OnOpen{info.ViewName});\r\n" +
							(info.IncluedDestroyEvent ? $"\t\t\tUnsubscribe<Destroy{info.ViewName}Event>(Destroy{info.ViewName}Event.EventId, OnDestroy{info.ViewName});\r\n" : "") +
							$"\t\t\tUnsubscribe<OnClose{info.ViewName}Event>(OnClose{info.ViewName}Event.EventId, OnClose{info.ViewName});");
						break;
					}
				}
				break;
			}
		}

		for (int i = lines.Count - 1; i >= 0; i--)
		{
			if (lines[i].StartsWith("}"))
			{
				for (int j = i - 1; j >= 0; j--)
				{
					if (lines[j].Contains("}"))
					{
						lines.Insert(j,
							$"\r\n" +
							$"\t\tprivate async void OnOpen{info.ViewName}(Open{info.ViewName}Event e)\r\n" +
							$"\t\t{{\r\n" +
								$"\t\t\tif(m_{info.ViewName}View != null)\r\n" +
								$"\t\t\t{{\r\n" +
									$"\t\t\t\treturn;\r\n" +
								$"\t\t\t}}\r\n" +
								$"\t\t\tm_{info.ViewName}View = await CreateView<{info.ViewName}>();\r\n" +
							$"\t\t}}\r\n\r\n");
						lines.Insert(j + 1,
							$"\t\tprivate void OnClose{info.ViewName}(OnClose{info.ViewName}Event e)\r\n" +
							$"\t\t{{\r\n" +
								$"\t\t\tm_{info.ViewName}View = null;\r\n" +
							$"\t\t}}");
						if (info.IncluedDestroyEvent)
						{
							lines.Insert(j + 1,
								$"\t\tprivate void OnDestroy{info.ViewName}(Destroy{info.ViewName}Event e)\r\n" +
								$"\t\t{{\r\n" +
									$"\t\t\tm_{info.ViewName}View?.Destroy();\r\n" +
								$"\t\t}}\r\n\r\n");
						}
					}
					break;
				}
				break;
			}
		}
		File.WriteAllLines(controllerPath, lines.ToArray());

		lines = new List<string>(File.ReadAllLines(modulePath));
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("public override void RegisterViewConfig()"))
			{
				for (int j = i + 1; j < lines.Count; j++)
				{
					if (lines[j].Contains("}"))
					{
						lines.Insert(j, $"\t\t\tRegisterView<{info.ViewName}, OnClose{info.ViewName}Event>(OnClose{info.ViewName}Event.Create);");
						break;
					}
				}
				break;
			}
		}
		File.WriteAllLines(modulePath, lines.ToArray());

		EditorUtility.DisplayDialog("添加View", $"创建 {info.ViewName}成功", "确认");
		AssetDatabase.Refresh();
	}
	#endregion
}
#pragma warning restore CS0162