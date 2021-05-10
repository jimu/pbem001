using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DisableButtonWhenActive : MonoBehaviour
{
    public Button button;

    private void OnEnable()
    {
        button.interactable = false;
    }

    private void OnDisable()
    {
        button.interactable = true;
    }
}
