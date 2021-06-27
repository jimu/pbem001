using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebApi;
using BopperApi;

/***
 * Retrieves MatchState for THE game for the authorized user
 */
public class GetMatchstate
{
    const string URL = "https://www.tripleplusungood.com/bopper/api/playauth/";
    // Start is called before the first frame update
    void Start()
    {
    }


    #region play
    static public void Get(int match_id)
    {
        if (!AuthToken.HasToken())
            Debug.Log("Error: no token");
        Request<PlayMatchResponse>.Get(GameManager.instance, URL + match_id, PlayCallback);
    }


    static void PlayCallback(PlayMatchResponse response, long statusCode)
    {
        Debug.Log($"Response: {response} status:{statusCode} m:{response.message} u:{(response.user != null ? response.user.ToString() : "NULL")}");
        if (statusCode >= 200 && statusCode <= 399 && response.user.avatar != null)
        {
            Debug.Log($"({statusCode})\nSuccess: {response.user.name}\nPlayer: {response.player_id}\nStatus: {response.status}\nMatch: {response.match.name}\nMatchstate:\n{response.matchstate.commands}");
            GameManager.instance.LoadMatchstateCommands(response.matchstate.commands);
        }
        else /* FAIL */
        {
            //avatar.Hide();
            Debug.Log($"({statusCode})\n{response.message}\n");
        }
    }
    #endregion play

}
