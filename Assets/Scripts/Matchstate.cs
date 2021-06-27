using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    static int NUM_UNIT_TYPES = 7;
    public int id;
    public string name;
    public UnitType[] pta = new UnitType[NUM_UNIT_TYPES];

    public Player(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public new string ToString()
    {
        string output = $"PLAYER {id}: {name}\nPTA:\n";

        for (UnitType utype = UnitType.LB; utype <= UnitType.BB; ++utype)
            output += $" - {utype} => {pta[(int)utype]}\n";

        return output;
    }
}

[Serializable]
public class Matchstate
{
    public List<Bopper.Unit>   units    = new List<Bopper.Unit>();
    public List<string> messages = new List<string>();
    public List<Player> players = new List<Player>();

    public Matchstate()
    {
        players.Add(new Player(0, "neutral"));
        players.Add(new Player(1, "white"));
        players.Add(new Player(2, "red"));
    }


    public void Dump()
    {
        string output = "";

        foreach (Player p in players)
            output += p.ToString();

        if (units.Count > 0)
        {
            output += "Units:\n";
            foreach (var unit in units)
                output += $" - {unit}\n"; 
        }

        if (messages.Count > 0)
            output += "Messages:\n - " + string.Join("\n - ", messages);

        Debug.Log(output);
    }
}
