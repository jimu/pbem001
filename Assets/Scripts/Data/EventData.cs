using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event", fileName = "New Event")]
public class EventData : ScriptableObject
{
    List<UnityEvent> events = new List<UnityEvent>();

    public void Raise()
    {
        Debug.Log($"Event ${this.name} Raised");
        foreach(var e in events)
            e.Invoke();
    }

    public void AddListener(UnityEvent unityEvent)
    {
        events.Add(unityEvent);
    }
}
