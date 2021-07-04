using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebApi;




public class Login : MonoBehaviour
{
    public TMP_InputField user;
    public TMP_InputField password;
    public Button submitButton;
    public TextMeshProUGUI statusText;
    public Button button;
    public Toggle rememberMe;
    public Authenticator auth;
    public GameObject logonPanel;
    public GameObject statusPanel;

    const string USER_KEY = "user";
    const string PASSWORD_KEY = "password";

    const int MIN_FIELD_LENGTH = 3;

    private bool isAuthenticated = false;

    public void Authenticate(Action<bool> callback)
    {
        if (isAuthenticated)
            callback(true);

        SetUIConnecting();

        auth.Authenticate((resp, status) =>
        {
            bool success = status >= 200 && status < 300;
            if (success || status == 401) // unauthorized
            {
                statusPanel.SetActive(false);
                logonPanel.SetActive(true);
            }
            else
                SetUIConnecting(false, resp.message, success);

            callback(success);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        RecallValues();
        CheckValidCredentials();
    }

    public void CheckValidCredentials()
    {
        submitButton.interactable = user.text.Trim().Length >= MIN_FIELD_LENGTH &&
            password.text.Trim().Length >= MIN_FIELD_LENGTH;
    }

    void RecallValues()
    {
        user.text = PlayerPrefs.GetString(USER_KEY);
        password.text = PlayerPrefs.GetString(PASSWORD_KEY);
        rememberMe.isOn = user.text.Length > 0;

        Debug.Log($"Recalled {user.text}:{password.text}:{AuthToken.GetToken()}");
    }

    public void Submit()
    {
        logonPanel.SetActive(false);
        SetUIConnecting();
        auth.Authenticate(user.text, password.text, CallbackLogin);
        if (rememberMe.isOn)
        {
            Debug.Log($"Saving {user.text}:{password.text}");
            PlayerPrefs.SetString(USER_KEY, user.text);
            PlayerPrefs.SetString(PASSWORD_KEY, password.text);
        }
        else
        {
            Debug.Log($"Clearing {user.text}:{password.text}");
            PlayerPrefs.DeleteKey(USER_KEY);
            PlayerPrefs.DeleteKey(PASSWORD_KEY);
        }

        //StartCoroutine(Send());
    }

    /*
    IEnumerator Send()
    {
        yield return new WaitForSeconds(5);
        SetUIConnecting(false);
    }
    */

    void SetUIConnecting(bool isConnecting = true, string message = "Connecting...", bool success = true)
    {
        statusPanel.SetActive(true);
        statusText.text = message;
        statusText.color = isConnecting ? Color.white : success ? Color.green : Color.red;
        statusText.GetComponent<Animator>().enabled = isConnecting;
        button.GetComponentInChildren<TextMeshProUGUI>().text = isConnecting ? "Cancel" : "Close";
    }


    void CallbackLogin(LoginResponse response, long statusCode)
    {
        Debug.Log($"Response: {response} status:{statusCode} m:{response.message} u:{(response.user != null ? response.user.ToString() : "NULL")}");
        bool success = statusCode >= 200 && statusCode <= 399;
        if (success && response.user.avatar != null)
        {
            //            avatar.Fetch(response.user.avatar);
            //            output.text = $"({statusCode})\nSuccess: {response.user.name}\nToken: {response.token}";
            if (response.token.Length > 0)
                AuthToken.SetToken(response.token);
        }
        SetUIConnecting(false, response.message, success);
    }

    [Obsolete("DisplayLoginDialogIfNotLoggedIn is deprecated")]

    public void DisplayLoginDialogIfNotLoggedIn()
    {
        if (!AuthToken.HasToken())
            logonPanel.SetActive(true);
    }

    // TODO: coupled to GameManger bad?
    public void DisplayLoginDialogIfNotLoggedInElsePlay()
    {
        if (!AuthToken.HasToken())
            logonPanel.SetActive(true);
        else
            GameManager.instance.SetState(GameState.Play);

    }
}