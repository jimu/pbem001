using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    public EventDataInt strengthChangedEvent;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        strengthChangedEvent.AddListener(StrengthChanged);
        button = GetComponent<Button>();
        button.interactable = false;
    }

    public void StrengthChanged(int value)
    {
        Debug.Log($"DisableButtonWhenActive({value})");
        button.interactable = value == 30;
    }

}
