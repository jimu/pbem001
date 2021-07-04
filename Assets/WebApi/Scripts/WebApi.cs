// ver 0.0.2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

namespace WebApi
{
    public static class AuthToken
    {
        const string TOKEN_KEY = "authtoken";

        private static string token = "";

        static public string SetToken(string token)
        {
            Debug.Log($"SetToken: {token}");
            PlayerPrefs.SetString(TOKEN_KEY, token);
            return AuthToken.token = token;
        }
        static public string GetToken()
        {
            if (AuthToken.token.Length < 1)
                AuthToken.token = PlayerPrefs.GetString(TOKEN_KEY);

            return AuthToken.token;
        }

        static public bool HasToken()
        {
            return GetToken().Length > 0;
        }
    }



    public enum Method { GET, POST, PUT, DELETE };

    static public class Request<T> where T : new()
    {

        static public void Call(MonoBehaviour mb, string url, string data, Action<T, long> callback)
        {
            Debug.Log($"Api.Call({url})");
            //Debug.Log($"Token ({token.Length}): \"{token}\"");

            mb.StartCoroutine(IEnumeratorCall(url, Method.POST, data, callback));
        }

        static public void Get(MonoBehaviour mb, string url, Action<T, long> callback)
        {
            Debug.Log($"Api.Get({url})");
            //Debug.Log($"Token ({token.Length}): \"{token}\"");

            mb.StartCoroutine(IEnumeratorCall(url, Method.GET, null, callback));
        }
        static public void Post(MonoBehaviour mb, string url, string data, Action<T, long> callback)
        {
            Debug.Log($"Api.Call({url})");
            //Debug.Log($"Token ({token.Length}): \"{token}\"");

            mb.StartCoroutine(IEnumeratorCall(url, Method.POST, data, callback));
        }


        static private IEnumerator IEnumeratorCall(string uri, Method method, string data, Action<T, long> callback)
        {
            UnityWebRequest request = method == Method.GET ?
                UnityWebRequest.Get(uri) :
                UnityWebRequest.Post(uri, data);

            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-type", "application/json");

            //Debug.Log($"Token ({Request<T>.token.Length}): \"{Request<T>.token}\"");
            if (AuthToken.HasToken())
                request.SetRequestHeader("Authorization", "Bearer " + AuthToken.GetToken());

            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text);

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error While Sending: " + request.error);

                callback(JsonUtility.FromJson<T>($"{{\"message\": \"{request.error}\"}}"), 500L);
            }
            else
            {
                callback(JsonUtility.FromJson<T>(request.downloadHandler.text), request.responseCode);
            }
        }
    }
}
