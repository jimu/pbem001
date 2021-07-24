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
	public int visableStart = 0;
	public int visableEnd = int.MaxValue;

	public new void InsertRange(int index, IEnumerable<T> list)
	{
		int count = Count;

		base.InsertRange(index, list);
		listeners.Invoke(index, Count - count);
	}

	public new void Insert(int index, T item)
	{
		base.Insert(index, item);
		listeners.Invoke(index, 1);
	}

	public new void Add(T item)
	{
        //Debug.Log($"Add called");
		base.Add(item);
		listeners.Invoke(Count - 1, 1);
	}


	public new void AddRange(IEnumerable<T> items)
	{
		base.AddRange(items);
		listeners.Invoke(0, Count);
	}


	public new void RemoveAt(int index)
	{
		base.RemoveAt(index);
		listeners.Invoke(index, -1);
	}
	public new void RemoveRange(int index, int count)
	{
		base.RemoveRange(index, count);
		listeners.Invoke(index, -count);
	}

	public new void Clear()
	{
		base.Clear();
		listeners.Invoke(0, 0);
	}

}