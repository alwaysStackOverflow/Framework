using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;
using UnityGameFramework;

namespace Client
{
#if UNITY_EDITOR
	using Sirenix.OdinInspector;

	[Serializable]
	public class ContainerItem
	{
		[HorizontalGroup("ContainerItem", MinWidth = 150, MaxWidth = 250)]
		[HideLabel, Delayed]
		[GUIColor("GetNameColor")]
		public string Key = string.Empty;

		[BoxGroup("ContainerItem/Value", false), HideLabel]
		[HideIf("@this.gameObject == null")]
		[ValueDropdown("GetAllComponent")]
		[InlineButton("ShowGameObject", "▼")]
		[SuffixLabel("@gameObject ? gameObject.name : string.Empty", true)]
		public UnityObject Value = null;

		[BoxGroup("ContainerItem/Value", false), HideLabel]
		[ShowIf("@gameObject == null || _isShowGameObject")]
		[OnValueChanged("OnGameObjectValueChange")]
		[GUIColor("GetGameObjectColor")]
		public GameObject gameObject;

		private bool _isShowGameObject;

		protected void ShowGameObject()
		{
			_isShowGameObject = !_isShowGameObject;
		}

		protected IEnumerable GetAllComponent()
		{
			if (gameObject)
			{
				yield return new ValueDropdownItem(gameObject.GetType().ToString().Split('.').Last(), gameObject);

				foreach (var component in gameObject.GetComponents<Component>())
				{
					yield return new ValueDropdownItem(component.GetType().ToString().Split('.').Last(), component);
				}
			}
		}

		protected void OnGameObjectValueChange()
		{
			if (string.IsNullOrWhiteSpace(Key) && gameObject)
			{
				Key = gameObject.name;
			}

			var oldObj = Value ? Value is GameObject obj ? obj : ((Component)Value).gameObject : null;
			if (oldObj != gameObject)
			{
				Value = gameObject;
			}
		}

		protected Color GetGameObjectColor()
		{
			return gameObject == null ? Color.red : Color.white;
		}

		protected Color GetNameColor()
		{
			return string.IsNullOrWhiteSpace(Key) ? Color.red : Color.white;
		}
	}

	[HideMonoScript]
	[Serializable]
	public partial class UnityObjectContainer : MonoBehaviour
	{
		[HorizontalGroup("UnityObjectContainerRemove")]
		[PropertyOrder(0)]
		[Button("移除所有空元素")]
		public void RemoveAllEmpty()
		{
			for (int i = 0; i < _list.Count;)
			{
				if (_list[i].Value == null || _list[i].gameObject == null || string.IsNullOrEmpty(_list[i].Key))
				{
					_list.RemoveAt(i);
				}
				else
				{
					++i;
				}
			}
		}
		[HorizontalGroup("UnityObjectContainerRemove")]
		[PropertyOrder(1)]
		[Button("移除所有元素")]
		public void RemoveAll()
		{
			_list.Clear();
		}

		[PropertyOrder(-1), Delayed]
		[LabelText("查找元素: "), OnValueChanged("OnSearch")]
		public string SearchName;

		public void OnSearch()
		{
			SearchList.Clear();
			if (string.IsNullOrEmpty(SearchName))
			{
				return;
			}
			foreach (ContainerItem item in _list)
			{
				if (!string.IsNullOrEmpty(item.Key) && item.Key.ToLower().Contains(SearchName.ToLower()))
				{
					SearchList.Add(item);
				}
			}
		}

		[PropertyOrder(3)]
		[HideIf("@string.IsNullOrEmpty(SearchName)")]
		[ListDrawerSettings(NumberOfItemsPerPage = 50, ShowPaging = true, Expanded = true, HideAddButton = true, HideRemoveButton = true)]
		public List<ContainerItem> SearchList = new();


		[PropertyOrder(99999)]
		[HideIf("@!string.IsNullOrEmpty(SearchName)")]
		[ListDrawerSettings(NumberOfItemsPerPage = 50, ShowPaging = true, Expanded = true)]
		[SerializeField]
		private List<ContainerItem> _list = new();
	}
#else
	public class ContainerItem
	{
		public string Key = string.Empty;

		public UnityObject Value = null;
	}

	[Serializable]
	public partial class UnityObjectContainer : MonoBehaviour
	{
		[SerializeField]
		private List<ContainerItem> _list = new();
	}
#endif

	public partial class UnityObjectContainer : MonoBehaviour
	{
		public static Dictionary<string, UnityObject> GetReference(UnityObject obj)
		{
			if (obj == null)
			{
				return new Dictionary<string, UnityObject>();
			}
			UnityObjectContainer container;
			if (obj is GameObject go)
			{
				container = go.GetComponent<UnityObjectContainer>();
			}
			else if (obj is Component comp)
			{
				container = comp.GetComponent<UnityObjectContainer>();
			}
			else
			{
				return new Dictionary<string, UnityObject>();
			}
			if (container == null)
			{
				return new Dictionary<string, UnityObject>();
			}
			var dic = new Dictionary<string, UnityObject>();
			foreach (var item in container._list)
			{
				if (string.IsNullOrWhiteSpace(item.Key) || item.Value == null)
				{
					continue;
				}
				if (!dic.ContainsKey(item.Key))
				{
					dic.Add(item.Key, item.Value);
				}
				else
				{
					Log.Warning($"UnityObjectContainer.GetReference ID:{item.Key} already Exist");
				}
			}
			return dic;
		}

		public static Dictionary<string, UnityObject> GetReference(UnityObjectContainer container)
		{
			if (container == null)
			{
				return new Dictionary<string, UnityObject>();
			}
			var dic = new Dictionary<string, UnityObject>();
			foreach (var item in container._list)
			{
				if (string.IsNullOrWhiteSpace(item.Key) || item.Value == null)
				{
					continue;
				}
				if (!dic.ContainsKey(item.Key))
				{
					dic.Add(item.Key, item.Value);
				}
				else
				{
					Log.Warning($"UnityObjectContainer.GetReference ID:{item.Key} already Exist");
				}
			}
			return dic;
		}
	}
}