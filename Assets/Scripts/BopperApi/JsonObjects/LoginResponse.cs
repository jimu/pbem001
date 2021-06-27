using System;


namespace BopperApi
{
    [Serializable]
    public class JsonLogin
    {

        public User user;      // user payload
        public string token;   // auth token
        public string message; // error message
    }
}