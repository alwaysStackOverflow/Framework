using System.Collections.Concurrent;

public class ConcurrentDoubleKeyMap<Tkey1, Tkey2, TValue> : ConcurrentDictionary<Tkey1, ConcurrentDictionary<Tkey2, TValue>>
{
	public ConcurrentDoubleKeyMap()
	{
	}

	public void Add(Tkey1 key1, Tkey2 key2, TValue value)
	{
		if (!TryGetValue(key1, out ConcurrentDictionary<Tkey2, TValue> values))
		{
			values = new ConcurrentDictionary<Tkey2, TValue>();
			TryAdd(key1, values);
		}

		values.TryAdd(key2, value);
	}

	public bool Remove(Tkey1 key1, Tkey2 key2)
	{
		if (!TryGetValue(key1, out ConcurrentDictionary<Tkey2, TValue> values))
		{
			return false;
		}

		if (!values.TryRemove(key2, out var _))
		{
			return false;
		}

		if (values.Count <= 0)
		{
			TryRemove(key1, out var _);
		}

		return true;
	}

	public bool Contains(Tkey1 key1, Tkey2 key2)
	{
		if (!TryGetValue(key1, out ConcurrentDictionary<Tkey2, TValue> values))
		{
			return false;
		}

		return values.ContainsKey(key2);
	}

	public bool TryGetValue(Tkey1 key1, Tkey2 key2, out TValue value)
	{
		if (TryGetValue(key1, out ConcurrentDictionary<Tkey2, TValue> values))
		{
			return values.TryGetValue(key2, out value);
		}

		value = default;
		return false;
	}

	public int TotalCount
	{
		get
		{
			int count = 0;
			foreach (var item in this)
			{
				count += item.Value.Count;
			}
			return count;
		}
	}
}
