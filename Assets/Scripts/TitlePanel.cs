using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebApi;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    [SerializeField] Button logoutButton;

    // Start is called before the first frame update
    void Start()
    {
        logoutButton.interactable = AuthToken.HasToken();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
