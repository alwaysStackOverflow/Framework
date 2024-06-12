using System.Collections.Generic;

public class UnSortedMultiMap<TKey, TValue> : Dictionary<TKey, HashSet<TValue>>
{
	public UnSortedMultiMap()
	{
	}

	public UnSortedMultiMap(int capacity) : base(capacity)
	{
	}

	public UnSortedMultiMap(IEqualityComparer<TKey> comparer) : base(comparer)
	{
	}

	public UnSortedMultiMap(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
	{
	}

	public void Add(TKey key, TValue value)
	{
		if (!TryGetValue(key, out HashSet<TValue> values))
		{
			values = new HashSet<TValue>();
			Add(key, values);
		}

		values.Add(value);
	}

	public bool Remove(TKey key, TValue value)
	{
		if (!TryGetValue(key, out HashSet<TValue> values))
		{
			return false;
		}

		if (!values.Remove(value))
		{
			return false;
		}

		if (values.Count <= 0)
		{
			Remove(key);
		}

		return true;
	}

	public bool Contains(TKey key, TValue value)
	{
		if (!TryGetValue(key, out HashSet<TValue> values))
		{
			return false;
		}

		return values.Contains(value);
	}

	public bool TryGetValue(TKey key, TValue value, out TValue outValue)
	{
		if (!TryGetValue(key, out HashSet<TValue> values))
		{
			outValue = default;
			return false;
		}

		return values.TryGetValue(value, out outValue);
	}

	public int TotalCount
	{
		get
		{
			int count = 0;
			foreach (var values in Values)
			{
				count += values.Count;
			}

			return count;
		}
	}
}
