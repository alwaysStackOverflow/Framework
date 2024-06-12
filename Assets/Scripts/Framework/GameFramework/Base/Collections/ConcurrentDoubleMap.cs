using System;
using System.Collections.Concurrent;

/// <summary>
/// 双向字典, K与V互相为Key和Value
/// </summary>
public class ConcurrentDoubleMap<TKey, TValue>
{
	private readonly ConcurrentDictionary<TKey, TValue> kv;
	private readonly ConcurrentDictionary<TValue, TKey> vk;

	public ConcurrentDoubleMap()
	{
		kv = new();
		vk = new();
	}

	public void Add(TKey key, TValue value)
	{
		if (key == null || value == null || kv.ContainsKey(key) || vk.ContainsKey(value))
		{
			return;
		}
		kv.TryAdd(key, value);
		vk.TryAdd(value, key);
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

		kv.TryRemove(key, out var _);
		vk.TryRemove(value, out var _);
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

		kv.TryRemove(key, out var _);
		vk.TryRemove(value, out var _);
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
