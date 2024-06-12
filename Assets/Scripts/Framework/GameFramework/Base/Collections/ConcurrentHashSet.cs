using System.Collections.Concurrent;

public class ConcurrentHashSet<T> : ConcurrentDictionary<T, T>
{
	public ConcurrentHashSet()
	{
		
	}

	public bool Add(T item)
	{
		return TryAdd(item, item);
	}

	public bool GetValue(T equalValue, out T value)
	{
		if (TryGetValue(equalValue, out value))
		{
			return true;
		}
		return false;
	}

	public bool Remove(T equalValue, out T value)
	{
		return TryRemove(equalValue, out value);
	}
}
