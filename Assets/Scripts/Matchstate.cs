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
    public string shortname;
    public UnitType[] pta = new UnitType[NUM_UNIT_TYPES];

    public Player(int id, string name)
    {
        this.id = id;
        this.name = name;
        this.shortname = name.Substring(0, 3).ToUpper();
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
    public CommandStatus commandStatus = new CommandStatus();

    public Matchstate()
    {
        players.Add(new Player(0, "neutral"));
        players.Add(new Player(1, "white"));
        players.Add(new Player(2, "red"));
    }

    [System.Obsolete]
    public Bopper.Unit findUnit(int player_id, string name)     // todo use guid
    {
        foreach (var unit in units)
            if (unit.name == name && unit.player_id == player_id)
                return unit;
        return null;
    }

    /// <summary>
    /// Dumps the matchstate
    /// Note:  COMPLETELY UNRELATED TO COMMAND LIST
    /// </summary>
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

    public void SetStatus(CommandStatus.Type type = CommandStatus.Type.None, long parameter1 = 0)
    {
        commandStatus.type = type;
        commandStatus.parameter1 = parameter1;
    }
}
