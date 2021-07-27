using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Bopper.View.Unity;



public class SubmitOrders : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI contentText;
    public TMPro.TMP_InputField inputField;

    const string SubmitUrl = "https://www.tripleplusungood.com/hello/write/";
    string error = "";

    [SerializeField] Button submitButton;
    public Button closeButton;


    
    private void OnEnable()
    {
        Debug.Log("SubmitOrders.OnEnable()");
        GenerateContent();
        ShowSubmitButton();
        ShowCloseButton(false);
        submitButton.interactable = true;
        statusText.text = "Ready";
    }

    private void GenerateContent()
    {
        string orders = "user markr\n";

        foreach (var hex in FindObjectsOfType<HexCell>())
            foreach (var unit in hex.units)
                orders += $"deploy {unit.data.code} {hex.coordinates.ToRivetsString()}\n";
        inputField.text = orders;
        contentText.text = orders;
        inputField.verticalScrollbar.value = 0;

        // see https://forum.unity.com/threads/multiline-input-field-with-scrollbar-scrolls-all-the-way-up-on-first-scroll.494388/
        var textComponent = inputField.textComponent;
        var textViewport = inputField.textViewport;
        textComponent.rectTransform.anchoredPosition = new Vector2(textComponent.rectTransform.anchoredPosition.x,
            (textComponent.preferredHeight - textViewport.rect.height) * 0);
    }


    public void Submit()
    {
        Debug.Log("Submit pressed");
        statusText.text = "Sending...";
        Send("markr", contentText.text);
        StartCoroutine(Send("markr", contentText.text));
    }


    public IEnumerator Send(string user, string content)
    {
        statusText.text = "Sending....";

        string uri = SubmitUrl + user;
        string data = $"user {user}\n{content}";


        using (UnityWebRequest webRequest = UnityWebRequest.Put(SubmitUrl + user, data))
        {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-type", "text/plain");

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    statusText.text = "Error: " + webRequest.error;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    statusText.text = "HTTP Error: " + webRequest.error;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    statusText.text = "Orders sent successfully";
                    ShowCloseButton();
                    ShowSubmitButton(false);
                    break;
            }
        }
    }

    private void ShowCloseButton(bool value = true)
    {
        closeButton.gameObject.SetActive(value);

    }

    private void ShowSubmitButton(bool value = true)
    {
        submitButton.gameObject.SetActive(value);
    }

    public string GetError()
    {
        return error;
    }
}
