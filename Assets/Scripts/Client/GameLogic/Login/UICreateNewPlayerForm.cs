using Common;
using GameFramework.Network;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework;

namespace Client
{
	public class UICreateNewPlayerForm : UIBaseForm
	{
		public class HeadIconData : EntityData
		{
			public GameObject Modle { get; set; }
			public PlayerModelType ModleType { get; set; }
			public string AssetPath { get; set; }
		}

		public class HeadIconEntity : Entity<UICreateNewPlayerForm, HeadIconData>
		{
			private Button _button;
			private UIImage _image;
			public HeadIconEntity(UICreateNewPlayerForm baseView, GameObject go, HeadIconData data) : base(baseView, go, data)
			{
				Init();
				Refresh();
			}

			protected override void OnInit()
			{
				_button = GetField<Button>("Button");
				_image = GetField<UIImage>("Image");
				BaseView.AddClick(_button, OnClick);
			}

			private string GetSpriteName()
			{
				switch(Data.ModleType)
				{
					case PlayerModelType.WhiteMale:
					{
						return "WhiteMale";
					}
					case PlayerModelType.YellowMale:
					{
						return "YellowMale";
					}
					case PlayerModelType.BrownMale:
					{
						return "BrownMale";
					}
					case PlayerModelType.BlackMale:
					{
						return "BlackMale";
					}
					case PlayerModelType.WhiteFemale:
					{
						return "WhiteFemale";
					}
					case PlayerModelType.YellowFemale:
					{
						return "WhiteMale";
					}
					case PlayerModelType.BrownFemale:
					{
						return "WhiteMale";
					}
					case PlayerModelType.BlackFemale:
					{
						return "WhiteMale";
					}
					default:
					{
						return "WhiteMale";
					}
				}
			}

			public override void Refresh()
			{
				_image.SetSprite(GetSpriteName());
			}

			public void OnClick()
			{
				BaseView.OnHeadIconEntityClick(Data.ModleType);
			}
		}

		public LoginData Data { get; private set; }
		public LoginController Controller { get; private set; }

		public UICreateNewPlayerForm() : base(UIType.UICreateNewPlayerForm)
		{
			Data = ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Model;
			Controller = ModuleManager.Instance.GetModule<LoginModule>(ModuleType.Login).Controller;
		}

		private GameObject _content;
		private GameObject _headIconPrefab;
		private GameObject _playerStyleShow;
		private Toggle _maleToggle;
		private Toggle _femaleToggle;
		private TMP_InputField _nameInput;
		private TMP_InputField _accountInput;
		private TMP_InputField _passwordInput;
		private GameObject _confirmBtn;

		protected override void OnInit()
		{
			InitUnityObject();
			InitEvent();
		}

		public void InitUnityObject()
		{
			_content = GetField("Content");
			_headIconPrefab = GetField("HeadIconPrefab");
			_playerStyleShow = GetField("PlayerStyleShow");
			_maleToggle = GetField<Toggle>("MaleToggle");
			_femaleToggle = GetField<Toggle>("FemaleToggle");
			_nameInput = GetField<TMP_InputField>("NameLabel");
			_accountInput = GetField<TMP_InputField>("AccountLabel");
			_passwordInput = GetField<TMP_InputField>("PasswordLabel");
			_confirmBtn = GetField("ConfirmBtn");
			GetField("AccountObj").SetActive(!Data.IsMainAccount);
			GetField("PasswordObj").SetActive(!Data.IsMainAccount);
			_headIconEntityList = new();
			_headIconDataList = new();
			_currentGenderType = GenderType.None;
			_currentShowType = PlayerModelType.None;
			_currentShowGameobject = null;
			_maleToggle.Select();
			OnGenderToggleClick(GenderType.Male);
		}

		public void InitEvent()
		{
			AddToogleEvent(_maleToggle, OnGenderToggleClick, GenderType.Male);
			AddToogleEvent(_femaleToggle, OnGenderToggleClick, GenderType.Female);
			AddClick(_confirmBtn, OnConfirmClick);
		}

		protected override void OnDestroy()
		{

		}

		private GenderType _currentGenderType;
		private void OnGenderToggleClick(GenderType toggleType)
		{
			if (_currentGenderType == toggleType)
			{
				return;
			}
			_currentGenderType = toggleType;
			switch (toggleType)
			{
				case GenderType.Male:
				{
					RefreshHeadIcon(Data.MaleList);
					break;
				}
				case GenderType.Female:
				{
					RefreshHeadIcon(Data.FemaleList);
					break;
				}
			}
		}

		private List<HeadIconEntity> _headIconEntityList;
		private Dictionary<PlayerModelType, HeadIconData> _headIconDataList;
		private void RefreshHeadIcon(List<PlayerModelType> modelTypeList)
		{
			foreach (var headIcon in _headIconEntityList)
			{
				headIcon.SetShow(false);
			}

			for (int i = 0; i < modelTypeList.Count; i++)
			{
				if (!_headIconDataList.TryGetValue(modelTypeList[i], out var data))
				{
					var config = ResourceConfig.Get(modelTypeList[i]);
					data = new HeadIconData()
					{
						ModleType = modelTypeList[i],
						AssetPath = config.AssetPath,
					};
					_headIconDataList.Add(modelTypeList[i], data);
				}
				if (i < _headIconEntityList.Count)
				{
					_headIconEntityList[i].UpdateData(data);
				}
				else
				{
					var go = _headIconPrefab.CloneSelf(_content, $"{i}");
					
					_headIconEntityList.Add(new HeadIconEntity(this, go, data));
				}
			}
			_headIconEntityList[0]?.OnClick();
		}

		private PlayerModelType _currentShowType;
		private GameObject _currentShowGameobject;
		private async void OnHeadIconEntityClick(PlayerModelType type)
		{
			_currentShowType = type;
			if (_headIconDataList.TryGetValue(type, out var data))
			{
				if(data.Modle == null)
				{
					data.Modle = await ResourceLoader.LoadGameObjectAsync(_playerStyleShow, data.AssetPath);
					data.Modle.SetLayerRecursively(GameCamera.GetLayer(UILayer.Entity));
					data.Modle.transform.localRotation = Quaternion.Euler(0, -180, 0);
					data.Modle.transform.localScale = new Vector3(250, 250, 250);
					data.Modle.transform.localPosition = new Vector3(0, -250, 0);
				}
				ShowModel(type, data.Modle);
			}
		}

		private void ShowModel(PlayerModelType type, GameObject go)
		{
			if(_currentShowType != type)
			{ 
				return;
			}
			_currentShowGameobject?.SetActive(false);
			go.SetActive(true);
			_currentShowGameobject = go;
		}

		private void OnConfirmClick()
		{
			if (!CheckName())
			{
				Log.Info("Name not fit");
				return;
			}
			if(!Data.IsMainAccount && !CheckAccount())
			{
				Log.Info("Account not fit");
				return;
			}
			if (!Data.IsMainAccount && !CheckPassword())
			{
				Log.Info("Password not fit");
				return;
			}
			if (Data.IsMainAccount)
			{
				MainAccountRegister();
			}
			else
			{
				NormalAccountRegister();
			}
		}

		private bool CheckName()
		{
			return !string.IsNullOrWhiteSpace(_nameInput.text) && _nameInput.text.Length < 18;
		}

		private bool CheckAccount()
		{
			return !string.IsNullOrWhiteSpace(_accountInput.text) && long.TryParse(_accountInput.text, out _);
		}

		private bool CheckPassword()
		{
			return !string.IsNullOrWhiteSpace(_passwordInput.text) && 5 < _passwordInput.text.Length && _passwordInput.text.Length < 18;
		}

		public void MainAccountRegister()
		{
			var request = CreateProtocolObject<MainAccountRegisterRequest>();
			request.Token = Data.Token;
			request.Name = _nameInput.text;
			request.Gender = _currentGenderType;
			request.ModelType = _currentShowType;
			SendNetworkMessage(request);
		}

		public void NormalAccountRegister()
		{
			var request = CreateProtocolObject<NormalAccountRegisterRequest>();
			request.Token = Data.Token;
			request.Name = _nameInput.text;
			request.Gender = _currentGenderType;
			request.ModelType = _currentShowType;
			request.Account = int.Parse(_accountInput.text);
			request.Password = _passwordInput.text;
			SendNetworkMessage(request);
		}
	}
}
