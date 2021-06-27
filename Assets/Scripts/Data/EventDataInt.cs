using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="IntEvent", fileName = "New IntEvent")]
public class EventDataInt : EventData<int>
{
}

public class EventData<T> : ScriptableObject
{
    public T value;
    
    UnityAction<T> actions;

    public void AddListener(UnityAction<T> a)
    {
        actions += a;
    }
    public void Raise(T value)
    {
        //Debug.Log($"Event raised({value})");
        this.value = value;
        if (actions != null)
            actions(value);
    }
}
