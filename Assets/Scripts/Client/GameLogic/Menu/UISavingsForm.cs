using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Client
{
	public class UISavingsForm : UIBaseForm
	{
		public class SavingData : EntityData
		{
			public FileInfo FileInfo { get; set; }
		}

		public class SavingEntity : Entity<UISavingsForm, SavingData>
		{
			private TextMeshProUGUI _name;
			private TextMeshProUGUI _lastPlayTime;
			public SavingEntity(UISavingsForm baseView, GameObject go, SavingData data) : base(baseView, go, data)
			{
				Init();
			}

			protected override void OnInit()
			{
				_name = GetField<TextMeshProUGUI>("Name");
				_lastPlayTime = GetField<TextMeshProUGUI>("LastPlayTime");
				BaseView.AddClick(GameObject, OnClick);
				BaseView.AddClick(GetField("DeleteBtn"), OnDeleteBtnClick);
			}

			public override void Refresh()
			{
				_name.text = "";
				_lastPlayTime.text = Data.FileInfo.LastWriteTime.ToString();
			}

			public void OnClick()
			{
				
			}

			public void OnDeleteBtnClick()
			{
				
			}
		}


		public MenuData Data { get; private set; }
		public MenuController Controller { get; private set; }
		private readonly List<SavingEntity> _savingEntities = new();
		public UISavingsForm() : base(UIType.UISavingsForm)
		{
			Data = ModuleManager.Instance.GetModule<MenuModule>(ModuleType.Menu).Model;
			Controller = ModuleManager.Instance.GetModule<MenuModule>(ModuleType.Menu).Controller;
		}

		protected override void OnInit()
		{
			InitUnityObject();
			InitEvent();
		}

		public void InitUnityObject()
		{
			var _recordInfoPrefab = GetField("RecordInfo");
			foreach (var info in Data.FileInfos)
			{
				var savingData = new SavingData()
				{
					FileInfo = info,
				};
				var go = _recordInfoPrefab.CloneSelf(gameObject, savingData.FileInfo.Name);
				_savingEntities.Add(new SavingEntity(this, go, savingData));
			}
		}

		public void InitEvent()
		{
			AddClick(GetField("BackBtn"), OnBackButtonClick);
		}

		protected override void OnDestroy()
		{

		}

		public void OnBackButtonClick()
		{
			Destroy();
		}
	}
}
