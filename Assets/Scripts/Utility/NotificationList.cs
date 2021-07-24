using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// List wrapper that notifies listeners when items are added, removed, or reset
/// 
/// <heading>Usage</heading>
///
///    var  list = new NotificationList<int>();
///    list.listeners += OnChange;
/// 
///    void OnChange(int index, int count) {...}
/// </summary>
/// <typeparam name="T"></typeparam>
public class NotificationList<T> : List<T>
{
	public Action<int, int> listeners;
	public int visibleStart = 1;
	public int visibleEnd = 3; // int.MaxValue;


	public new T this[int index]
	{
		get { return base[index + visibleStart]; }
	}

	public new int Count
	{
		get { return Math.Min(visibleEnd, base.Count) - visibleStart; }
	}

	public new void InsertRange(int index, IEnumerable<T> list)
	{
		int count = base.Count;

		base.InsertRange(index, list);
		NotifyListenersOfVisibleChanges(index, base.Count - count);
	}

	public new void Insert(int index, T item)
	{
		base.Insert(index, item);
		NotifyListenersOfVisibleChanges(index, 1);
	}

	public new void Add(T item)
	{
		Debug.Log($"Add called");
		base.Add(item);
		NotifyListenersOfVisibleChanges(base.Count - 1, 1);

	}


	public new void AddRange(IEnumerable<T> items)
	{
		base.AddRange(items);
		NotifyListenersOfVisibleChanges(0, base.Count);
	}


	public new void RemoveAt(int index)
	{
		base.RemoveAt(index);
		NotifyListenersOfVisibleChanges(index, 1, true);

	}
	public new void RemoveRange(int index, int count)
	{
		base.RemoveRange(index, count);
		NotifyListenersOfVisibleChanges(index, count, true);
	}

	public new void Clear()
	{
		base.Clear();
		listeners?.Invoke(0, 0);
	}

	private void NotifyListenersOfVisibleChanges(int start, int count, bool isDelete = false)
	{
		Debug.Log($"NotifyListenersOfVisibleChanges({start},{count},{isDelete})");
		if (start < visibleStart)
		{
			count -= visibleStart - start;
			start = visibleStart;
		}
		if (start + count > visibleEnd)
		{
			count = visibleEnd - start;
		}

		if (count > 0)
			listeners?.Invoke(start - visibleStart, count * (isDelete ? -1 : 1));
	}

	public bool AreMore()
	{
		return visibleEnd < base.Count;
	}

	public int RealToVisibleIndex(int real)
    {
		return real - visibleStart;
    }

	public int Reveal(int revealCount)
	{
		revealCount = Math.Min(revealCount, base.Count - visibleEnd);
		if (revealCount > 0)
		{
			int start = visibleEnd;
			visibleEnd += revealCount;
			NotifyListenersOfVisibleChanges(start, revealCount);
			return revealCount;
		}
		return 0;
	}
}