using System;

namespace BopperApi
{
    [Serializable]
    public class PlayMatchResponse
    {
        public Match match;
        public Matchstate matchstate;
        public User user;
        public int player_id;
        public int status;
        public string message;
    }
}