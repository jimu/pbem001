using System;
using System.Collections;
using UnityEngine;
using WebApi;



[Serializable]
public class User
{
    public int id;
    public string name;
    public string email;
    public string avatar;
    public string message;
}

[Serializable]
public class LoginResponse
{
    public User user;      // user payload
    public string token;   // auth token
    public string message; // error message
}



[Serializable]
public class Authenticator : MonoBehaviour
{
    const int MIN_LENGTH = 3;

    public bool ValidUSpec(string uspec)
    {
        return uspec.Length >= MIN_LENGTH;
    }

    public bool ValidPassword(string password)
    {
        return password.Length >= MIN_LENGTH;
    }

    public void Authenticate(string uspec, string password, Action<LoginResponse, long> callback)
    {
        string uri = "https://www.tripleplusungood.com/bopper/api/login?name=" + uspec + "&password=" + password;
        Debug.Log($"TestLogin({uri})");
        Request<LoginResponse>.Post(this, uri, "{\"name\":\"" + uspec + "\",\"password\":\"" + password + "\"}", callback);
    }

}
