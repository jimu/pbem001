using System;

namespace BopperApi
{
    [Serializable]
    public class Matchstate
    {
        public int id;
        public string match_id;
        public string turn;
        public string player_id;
        public string commands;
        public string message;
    }
}