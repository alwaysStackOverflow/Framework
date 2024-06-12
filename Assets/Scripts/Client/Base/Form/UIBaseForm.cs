using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework;
using UnityObject = UnityEngine.Object;
using System;
using GameFramework;

namespace Client
{
	public abstract class UIBaseForm : AView, IComparable<UIBaseForm>
	{
		private Canvas _mainCanvas = null;
		private CanvasGroup _canvasGroup = null;
		private List<Canvas> _canvasList = new();
		private Dictionary<string, UnityObject> _objects = new();
		public GameEventArgs DestroyEvent { get; set; }
		public bool IsDestroyed { get; private set; }

		public int DepthOffset
		{
			get;
			private set;
		}

		public int Depth
		{
			get
			{
				return _mainCanvas.sortingOrder;
			}
		}

		public readonly UIType ResourceType;

		public GameObject gameObject;

		protected T GetComponent<T>() where T : Component
		{
			return gameObject?.GetComponent<T>();
		}

		protected T GetField<T>(string name) where T : UnityObject
		{
			if (_objects.TryGetValue(name, out var obj))
			{
				if (obj is T)
				{
					return obj as T;
				}
			}
			return null;
		}

		protected GameObject GetField(string name)
		{
			if (_objects.TryGetValue(name, out var obj))
			{
				if (obj is GameObject)
				{
					return obj as GameObject;
				}
				else if(obj is Component)
				{
					return (obj as Component).gameObject;
				}
			}
			return null;
		}

		protected bool TryGetField<T>(string name, out T value) where T : UnityObject
		{
			if (_objects.TryGetValue(name, out var obj))
			{
				if (obj is T)
				{
					value = obj as T;
					return true;
				}
			}
			value = null;
			return false;
		}

		public UIBaseForm(UIType resourceType)
		{
			ResourceType = resourceType;
			IsDestroyed = true;
			DepthOffset = 0;
		}

		public async Task LoadAsync()
		{
			var resourceDataInfo = ResourceConfig.Get(ResourceType);
			if (resourceDataInfo == null)
			{
				Log.Error($"UIBaseForm Load fail, {ResourceType} not Exist");
				await Task.CompletedTask;
				return;
			}
			if (FormManager.Instance.HasForm(ResourceType))
			{
				Log.Error($"已经存在当前panel, type = {ResourceType}");
				await Task.CompletedTask;
				return;
			}
			gameObject = await ResourceLoader.LoadGameObjectAsync(GameCamera.Instance.Root, ResourceType);
			if (gameObject == null)
			{
				Log.Error($"UIBaseForm Load fail, Instantiate {ResourceType} gameObject is null");
				return;
			}
			gameObject.GetComponentsInChildren(true, _canvasList);
			foreach (var canvas in _canvasList)
			{
				canvas.renderMode = RenderMode.ScreenSpaceCamera;
				canvas.worldCamera = GameCamera.Instance.GetCamera(resourceDataInfo.UILayer);
				canvas.planeDistance = 0;
				canvas.overrideSorting = true;
			}
			_mainCanvas = gameObject.GetOrAddComponent<Canvas>();
			_canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
			FormManager.Instance.AddForm(this);
			IsDestroyed = false;
			gameObject.SetLayerRecursively(GameCamera.GetLayer(resourceDataInfo.UILayer));
			gameObject.SetActive(true);
			gameObject.GetOrAddComponent<GraphicRaycaster>();
			Init();
		}

		private void Init()
		{
			DepthOffset = _mainCanvas.sortingOrder;
			RectTransform transform = GetComponent<RectTransform>();
			transform.anchorMin = Vector2.zero;
			transform.anchorMax = Vector2.one;
			transform.anchoredPosition = Vector2.zero;
			transform.sizeDelta = Vector2.zero;
			gameObject.GetOrAddComponent<UnityObjectContainer>();
			_objects = UnityObjectContainer.GetReference(gameObject);
			OnInit();
		}

		protected virtual void OnInit()
		{

		}

		public void Show()
		{
			gameObject.SetActive(true);
			OnShow();
		}

		protected virtual void OnShow()
		{

		}

		public void Hide()
		{
			OnHide();
			gameObject.SetActive(false);
		}

		protected virtual void OnHide()
		{

		}

		public void Destroy()
		{
			if (IsDestroyed)
			{
				return;
			}
			IsDestroyed = true;
			FormManager.Instance.RemoveForm(ResourceType);
			OnDestroy();
			DestroyEvent?.Invoke();
			UnityObject.Destroy(gameObject);
			gameObject = null;
		}

		protected virtual void OnDestroy()
		{

		}

		public void Update(float elapseSeconds, float realElapseSeconds)
		{
			OnUpdate(elapseSeconds, realElapseSeconds);
		}

		protected virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{

		}

		public int AddDepth(int depthOffset)
		{
			
			for (int i = 0; i < _canvasList.Count; i++)
			{
				_canvasList[i].sortingOrder -= DepthOffset;
			}
			int maxDepth = int.MinValue;
			for (int i = 0; i < _canvasList.Count; i++)
			{
				_canvasList[i].sortingOrder += depthOffset;
				if(maxDepth < _canvasList[i].sortingOrder)
				{
					maxDepth = _canvasList[i].sortingOrder;
				}
			}
			_canvasList.Clear();
			DepthOffset = depthOffset;
			return maxDepth;
		}

		public int CompareTo(UIBaseForm other)
		{
			if (other == null)
			{
				return 1;
			}
			return DepthOffset - other.DepthOffset;
		}


		public void AddClick(GameObject go, GameFrameworkAction action)
		{
			var button = go.GetOrAddComponent<Button>();
			AddClick(button, action);
		}
		public void AddClick(Component comp, GameFrameworkAction action)
		{
			var button = comp.gameObject.GetOrAddComponent<Button>();
			AddClick(button, action);
		}
		public void AddClick(Button button, GameFrameworkAction action)
		{
			button.onClick.AddListener(delegate
			{
				action.Invoke();
			});
		}
		public void AddClick<T1>(GameObject go, GameFrameworkAction<T1> action, T1 arg1)
		{
			var button = go.GetOrAddComponent<Button>();
			AddClick(button, action, arg1);
		}
		public void AddClick<T1>(Component comp, GameFrameworkAction<T1> action, T1 arg1)
		{
			var button = comp.gameObject.GetOrAddComponent<Button>();
			AddClick(button, action, arg1);
		}
		public void AddClick<T1>(Button button, GameFrameworkAction<T1> action, T1 arg1)
		{
			button.onClick.AddListener(delegate
			{
				action.Invoke(arg1);
			});
		}
		public void AddClick<T1, T2>(GameObject go, GameFrameworkAction<T1, T2> action, T1 arg1, T2 arg2)
		{
			var button = go.GetOrAddComponent<Button>();
			AddClick(button, action, arg1, arg2);
		}
		public void AddClick<T1, T2>(Component comp, GameFrameworkAction<T1, T2> action, T1 arg1, T2 arg2)
		{
			var button = comp.gameObject.GetOrAddComponent<Button>();
			AddClick(button, action, arg1, arg2);
		}
		public void AddClick<T1, T2>(Button button, GameFrameworkAction<T1, T2> action, T1 arg1, T2 arg2)
		{
			button.onClick.AddListener(delegate
			{
				action.Invoke(arg1, arg2);
			});
		}

		public void AddToogleEvent(GameObject go, GameFrameworkAction<bool> action)
		{
			var toggle = go.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action);
		}
		public void AddToogleEvent(Component comp, GameFrameworkAction<bool> action)
		{
			var toggle = comp.gameObject.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action);
		}
		public void AddToogleEvent(Toggle toggle, GameFrameworkAction<bool> action)
		{
			toggle.onValueChanged.AddListener(delegate (bool isOn)
			{
				action.Invoke(isOn);
			});
		}
		public void AddToogleEvent<T1>(GameObject go, GameFrameworkAction<bool, T1> action, T1 arg1)
		{
			var toggle = go.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1);
		}
		public void AddToogleEvent<T1>(Component comp, GameFrameworkAction<bool, T1> action, T1 arg1)
		{
			var toggle = comp.gameObject.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1);
		}
		public void AddToogleEvent<T1>(Toggle toggle, GameFrameworkAction<bool, T1> action, T1 arg1)
		{
			toggle.onValueChanged.AddListener(delegate (bool isOn)
			{
				action.Invoke(isOn, arg1);
			});
		}
		public void AddToogleEvent<T1, T2>(GameObject go, GameFrameworkAction<bool, T1, T2> action, T1 arg1, T2 arg2)
		{
			var toggle = go.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1, arg2);
		}
		public void AddToogleEvent<T1, T2>(Component comp, GameFrameworkAction<bool, T1, T2> action, T1 arg1, T2 arg2)
		{
			var toggle = comp.gameObject.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1, arg2);
		}
		public void AddToogleEvent<T1, T2>(Toggle toggle, GameFrameworkAction<bool, T1, T2> action, T1 arg1, T2 arg2)
		{
			toggle.onValueChanged.AddListener(delegate (bool isOn)
			{
				action.Invoke(isOn, arg1, arg2);
			});
		}

		public void AddToogleEvent(GameObject go, GameFrameworkAction action)
		{
			var toggle = go.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action);
		}
		public void AddToogleEvent(Component comp, GameFrameworkAction action)
		{
			var toggle = comp.gameObject.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action);
		}
		public void AddToogleEvent(Toggle toggle, GameFrameworkAction action)
		{
			toggle.onValueChanged.AddListener(delegate (bool isOn)
			{
				if (isOn)
				{
					action.Invoke();
				}
			});
		}
		public void AddToogleEvent<T1>(GameObject go, GameFrameworkAction<T1> action, T1 arg1)
		{
			var toggle = go.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1);
		}
		public void AddToogleEvent<T1>(Component comp, GameFrameworkAction<T1> action, T1 arg1)
		{
			var toggle = comp.gameObject.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1);
		}
		public void AddToogleEvent<T1>(Toggle toggle, GameFrameworkAction<T1> action, T1 arg1)
		{
			toggle.onValueChanged.AddListener(delegate (bool isOn)
			{
				if (isOn)
				{
					action.Invoke(arg1);
				}
			});
		}
		public void AddToogleEvent<T1, T2>(GameObject go, GameFrameworkAction<T1, T2> action, T1 arg1, T2 arg2)
		{
			var toggle = go.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1, arg2);
		}
		public void AddToogleEvent<T1, T2>(Component comp, GameFrameworkAction<T1, T2> action, T1 arg1, T2 arg2)
		{
			var toggle = comp.gameObject.GetOrAddComponent<Toggle>();
			AddToogleEvent(toggle, action, arg1, arg2);
		}
		public void AddToogleEvent<T1, T2>(Toggle toggle, GameFrameworkAction<T1, T2> action, T1 arg1, T2 arg2)
		{	
			toggle.onValueChanged.AddListener(delegate (bool isOn)
			{
				if (isOn)
				{
					action.Invoke(arg1, arg2);
				}
			});
		}
	}
}
