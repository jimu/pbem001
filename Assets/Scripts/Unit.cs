using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using RMatch = System.Text.RegularExpressions.Match;


namespace Bopper
{
    [Serializable]
    public class Unit
    {
        public int id;
        public int coord;
        public int player_id;
        public string name;
        public UnitData data;
        public int layer;
        public long guid;

        static List<Unit> units = new List<Unit>();
        static Dictionary<long, Unit> guidToUnit = new Dictionary<long, Unit>();

        public Unit(UnitType utype, int player_id, int coord, int layer)
        {
            Debug.Log($"Unit({utype}) CTOR");
            Debug.Assert(utype > 0, "UnitType must be > 0");
            this.player_id = player_id;
            this.coord = coord;
            this.layer = layer;
            SetData(utype);
            Debug.Log($"Data is {this.data}");

            id = units.Count;
            int num = id; // todo
            name = $"{utype}-{num}";

            units.Add(this);
            guidToUnit[Guid(player_id, utype, num)] = this;
        }

        static public long Guid(int player_id, string unit_id)
        {
            RMatch match = Regex.Match(unit_id, @"(LB|RB|JB|BB|TB|BCPC)-?(\d+)");
            if (match.Success)
            {
                UnitType utype = Command.StringToUnitType(match.Groups[0].Value);
                int num = Int32.Parse(match.Groups[1].Value);
                return player_id * 100000 +(int)utype * 10000 + num;
            }
            return -1;
        }

        static public long Guid(int player_id, UnitType utype, int num)
        {
            return player_id * 100000 + (int)utype * 10000 + num;
        }

        private void SetData(UnitType utype)
        {
            data = GameManager.instance.gameData.unitData[(int)utype];
            if (data == null)
                throw new Exception("GameManager GameData UnitData entry missing for {utype}");
        }


        public override string ToString()
        {
            return $"[{name} owner:{player_id} coord:{coord}]";
        }
    }

}