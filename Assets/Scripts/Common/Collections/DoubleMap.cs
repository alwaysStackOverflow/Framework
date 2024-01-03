using System;
using System.Collections.Generic;

/// <summary>
/// 双向字典, K与V互相为Key和Value
/// </summary>
public class DoubleMap<TKey, TValue>
{
	private readonly Dictionary<TKey, TValue> kv;
	private readonly Dictionary<TValue, TKey> vk;

	public DoubleMap()
	{
		kv = new();
		vk = new();
	}

	public DoubleMap(int capacity)
	{
		kv = new(capacity);
		vk = new(capacity);
	}

	public void ForEach(Action<TKey, TValue> action)
	{
		if (action == null)
		{
			return;
		}
		Dictionary<TKey, TValue>.KeyCollection keys = kv.Keys;
		foreach (TKey key in keys)
		{
			action(key, kv[key]);
		}
	}

	public List<TKey> Keys
	{
		get
		{
			return new List<TKey>(kv.Keys);
		}
	}

	public List<TValue> Values
	{
		get
		{
			return new List<TValue>(vk.Keys);
		}
	}

	public void Add(TKey key, TValue value)
	{
		if (key == null || value == null || kv.ContainsKey(key) || vk.ContainsKey(value))
		{
			return;
		}
		kv.Add(key, value);
		vk.Add(value, key);
	}

	public TValue GetValueByKey(TKey key)
	{
		if (key != null && kv.ContainsKey(key))
		{
			return kv[key];
		}
		return default;
	}

	public TKey GetKeyByValue(TValue value)
	{
		if (value != null && vk.ContainsKey(value))
		{
			return vk[value];
		}
		return default;
	}

	public void RemoveByKey(TKey key)
	{
		if (key == null)
		{
			return;
		}
		if (!kv.TryGetValue(key, out TValue value))
		{
			return;
		}

		kv.Remove(key);
		vk.Remove(value);
	}

	public void RemoveByValue(TValue value)
	{
		if (value == null)
		{
			return;
		}

		if (!vk.TryGetValue(value, out TKey key))
		{
			return;
		}

		kv.Remove(key);
		vk.Remove(value);
	}

	public void Clear()
	{
		kv.Clear();
		vk.Clear();
	}

	public bool ContainsKey(TKey key)
	{
		if (key == null)
		{
			return false;
		}
		return kv.ContainsKey(key);
	}

	public bool ContainsValue(TValue value)
	{
		if (value == null)
		{
			return false;
		}
		return vk.ContainsKey(value);
	}

	public bool Contains(TKey key, TValue value)
	{
		if (key == null || value == null)
		{
			return false;
		}
		return kv.ContainsKey(key) && vk.ContainsKey(value);
	}
}
