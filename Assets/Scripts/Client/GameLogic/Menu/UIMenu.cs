using System;
using Common;
using System.IO;
using UnityEngine;
using GameFramework;
using UnityGameFramework;

namespace Client
{
	public class UIMenu : UIBaseForm
	{
		public MenuData Data { get; private set; }
		public MenuController Controller { get; private set; }
		public UIMenu() : base(UIType.UIMenuForm)
		{
			Data = ModuleManager.Instance.GetModule<MenuModule>(ModuleType.Menu).Model;
			Controller = ModuleManager.Instance.GetModule<MenuModule>(ModuleType.Menu).Controller;
		}

		protected override void OnInit()
		{
			InitUnityObject();
			InitEvent();
		}

		private GameObject _startNewGameButtonGo = null;
		private GameObject _resumeGameButtonGo = null;
		private GameObject _loadHistroyGameButtonGo = null;
		private GameObject _settingButtonButtonGo = null;
		private GameObject _quitButtonGo = null;

		public void InitUnityObject()
		{
			_startNewGameButtonGo = GetField("StartNewGameButton");
			_resumeGameButtonGo = GetField("ResumeGameButton");
			_loadHistroyGameButtonGo = GetField("LoadHistroyGameButton");
			_settingButtonButtonGo = GetField("SettingButton");
			_quitButtonGo = GetField("QuitButton");
			_resumeGameButtonGo.SetActive(Data.FileInfos.Count > 0);
			_loadHistroyGameButtonGo.SetActive(Data.FileInfos.Count > 0);
		}

		public void InitEvent()
		{
			AddClick(_startNewGameButtonGo, OnStartNewGameButtonClick);
			AddClick(_resumeGameButtonGo, OnResumeGameButtonClick);
			AddClick(_loadHistroyGameButtonGo, OnLoadHistroyGameButtonClick);
			AddClick(_settingButtonButtonGo, OnSettingButtonClick);
			AddClick(_quitButtonGo, OnQuitButtonClick);
		}

		protected override void OnDestroy()
		{

		}

		public void OnStartNewGameButtonClick()
		{
			AwakeServerEvent.Fire($"{Data.GameDatabaseDirectory}/{TimeInfo.LocalTime:yyyy_MM_dd_HH_mm_ss}.db");
		}

		public void OnResumeGameButtonClick()
		{
			long time = 0;
			FileInfo fileInfo = null;
			var list = Data.FileInfos;
			foreach (var info in list)
			{
				if (info.LastWriteTime.Ticks > time)
				{
					time = info.LastWriteTime.Ticks;
					fileInfo = info;
				}
			}
			AwakeServerEvent.Fire(fileInfo.FullName);
		}

		public void OnLoadHistroyGameButtonClick()
		{
			OpenUISavingsFormEvent.Fire();
		}

		public void OnSettingButtonClick()
		{

		}

		public void OnQuitButtonClick()
		{
			Log.Info("Quit");
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif

		}
	}
}
