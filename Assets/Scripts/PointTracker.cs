using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bopper.View.Unity;

public class PointTracker : MonoBehaviour
{
    public int MAX_STRENGTH = 30;
    public EventDataInt strengthChangedEvent;

    public void UpdatePoints()
    {
        int totalStrength = 0;
        foreach(Unit unit in FindObjectsOfType<Unit>())
            if (unit != null)
                totalStrength += unit.data.strength;

        Text text = GetComponent<Text>();
        text.text = totalStrength.ToString();
        text.color = totalStrength == MAX_STRENGTH ? Color.green :
            totalStrength <= MAX_STRENGTH ? Color.black :
            Color.red;

        strengthChangedEvent?.Raise(totalStrength);
    }
}
