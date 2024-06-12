using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace Client
{
	public abstract class EntityData
	{
		
	}

	public abstract class Entity<TBaseView, TData> where TData : EntityData
	{

		public Entity(TBaseView baseView,GameObject gameObject, TData data)
		{
			BaseView = baseView;
			GameObject = gameObject;
			Data = data;
		}
		public TBaseView BaseView { get; private set; }
		public GameObject GameObject { get; protected set; }

		public TData Data { get; protected set; }

		private Dictionary<string, UnityObject> _fields;

		public GameObject GetField(string key)
		{
			if(_fields.TryGetValue(key, out var obj))
			{
				if(obj is GameObject go)
				{
					return go;
				}
				else if(obj is Component comp)
				{
					return comp.gameObject;
				}
			}
			return null;
		}

		public TUnityObject GetField<TUnityObject>(string key) where TUnityObject : UnityObject
		{
			if (_fields.TryGetValue(key, out var obj))
			{
				if (obj is TUnityObject o)
				{
					return o;
				}
			}
			return null;
		}

		public virtual void Init()
		{
			_fields = UnityObjectContainer.GetReference(GameObject);
			SetShow(true);
			OnInit();
		}

		protected abstract void OnInit();

		public virtual void Update()
		{

		}

		public virtual void Release()
		{
			GameObject = null;
			Data = null;
		}

		public void UpdateData(TData data)
		{
			Data = data;
			SetShow(true);
			Refresh();
		}

		public abstract void Refresh();

		public virtual void SetShow(bool show)
		{
			GameObject?.SetActive(show);
		}
	}
}
