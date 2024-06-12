using GameFramework;
using GameFramework.Network;
using GameFramework.Procedure;
using System;
using UnityEngine;
using UnityGameFramework;
using UnityObject = UnityEngine.Object;

namespace Client
{
	public static class MethodExtension
	{
		public static GameObject CloneSelf(this GameObject go, GameObject parent, string name)
		{
			if(parent == null)
			{
				Log.Error("CloneSelf parent is null");
			}
			return CloneSelf(go, parent.transform, name);
		}

		public static GameObject CloneSelf(this GameObject go, Component parent, string name)
		{
			if (parent == null)
			{
				Log.Error("CloneSelf parent is null");
			}
			return CloneSelf(go, parent.transform, name);
		}

		public static GameObject CloneSelf(this GameObject go, Transform parent, string name)
		{
			if (parent == null)
			{
				Log.Error("CloneSelf parent is null");
			}
			var obj = UnityObject.Instantiate(go, parent, false);
			obj.name = name;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			return obj;
		}

		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
		}

		public static Component GetOrAddComponent(this GameObject gameObject, Type type)
		{
			return gameObject.GetComponent(type) ?? gameObject.AddComponent(type);
		}

		public static void SetActive(this Component comp, bool active)
		{
			comp.gameObject.SetActive(active);
		}

		/// <summary>
		/// argb
		/// </summary>
		public static Color CreateColor(this string hexString, bool includeAlpha = false)
		{
			if(uint.TryParse(hexString, out var colornum))
			{
				return CreateColor(colornum, includeAlpha);
			}
			return Color.white;
		}

		/// <summary>
		/// argb
		/// </summary>
		public static Color CreateColor(this int colornum, bool includeAlpha = false)
		{
			return CreateColor((uint)colornum, includeAlpha);
		}

		/// <summary>
		/// argb
		/// </summary>
		public static Color CreateColor(this uint colornum, bool includeAlpha = false)
		{
			var mod = 0xff;
			var a = (colornum >> 24) & mod;
			var r = (colornum >> 16) & mod;
			var g = (colornum >> 8) & mod;
			var b = colornum & mod;
			return new Color(r / 255f, g / 255f, b / 255f, includeAlpha ? a / 255f : 1f);
		}
	}
}
