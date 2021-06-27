using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Text.RegularExpressions;

// Commands
// deploy <type> <coord>
// pta <type> <type>
// say <message>




abstract public class Command
{
    static public string token;
    static public View view;
    public int player_id;

    abstract public Matchstate Execute(Matchstate state);
    abstract public Matchstate Undo(Matchstate state);

    /**
     * Helper
     */
    static public UnitType StringToUnitType(string value)
    {
        value = value.ToUpper();
        if (value == "BCPC")
            return UnitType.BPCPI;
        return (UnitType)Enum.Parse(typeof(UnitType), value);
    }

}


static public class CommandFactory
{
    public static Command Make(string line)
    {
        var tokens = Regex.Split(line, "\\s+");
        /* int player_id = Int32.Parse(tokens[0]);*/
        string cmd = tokens[1].ToUpper();

        return 
            cmd == CommandPName.token ? new CommandPName(tokens) :
            cmd == CommandSay.token ? new CommandSay(tokens) :
            cmd == CommandPTA.token ? new CommandPTA(tokens) :
            cmd == CommandDeploy.token ? new CommandDeploy(tokens) :
            (Command)null;
    }
}

public class CommandPName : Command
{
    static public new string token = "PNAME";
    private string name;
    private string previousName;

    public CommandPName(int player_id, string name)
    {
        this.player_id = player_id;
        this.name = name;
    }

    public CommandPName(string[] tokens)
    {
        player_id = Int32.Parse(tokens[0]);
        name = string.Join(" ", tokens, 2, tokens.Length - 2);   // Regex.Match(line, @"PNAME (.+)", RegexOptions.IgnoreCase).Groups[1].Value;
    }

    public CommandPName(string line)
    {
        var match = Regex.Match(line, @"(\d+) PNAME (.+)", RegexOptions.IgnoreCase);
        player_id = Int32.Parse(match.Groups[0].Value);
        name = match.Groups[1].Value;
    }

    override public Matchstate Execute(Matchstate state)
    {
        previousName = state.players[player_id].name;

        state.players[player_id].name = name;
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        state.players[player_id].name = previousName;
        return state;
    }

    public override string ToString()
    {
        return $"PName from {previousName} to {name}";
    }
}



public class CommandDeploy : Command
{
    static public new string token = "DEPLOY";
    public UnitType unitType;
    public int coord;
    public int layer;
    public int index;
    public Bopper.Unit unit;


    public CommandDeploy(int player_id, UnitType unitType, int hex)
    {

    }

    public CommandDeploy(string[] tokens)
    {
        player_id = Int32.Parse(tokens[0]);
        unitType = StringToUnitType(tokens[2]);
        coord = Int32.Parse(tokens[3]);
        layer = tokens.Length > 4 ? Int32.Parse(tokens[4]) : 0;
    }

    override public Matchstate Execute(Matchstate state)
    {
        Debug.Log(ToString());
        unit = new Bopper.Unit(unitType, player_id, coord, layer);
        Debug.Log($"{unit} utype={unit.data.unitType}");
        index = state.units.Count;
        state.units.Add(unit);
        view.DeployUnit(unit); // must pass on unitType, player_id, id, name, layer
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        view.UndeployUnit(unit);
        state.units.RemoveAt(index);
        return state;
    }
    
    public override string ToString()
    {
        return layer > 0 ?
            $"CommandDeploy[{player_id},{unitType},{coord},{layer}]" :
            $"CommandDeploy[{player_id},{unitType},{coord}]";
    }
}




public class CommandSay : Command
{
    static public new string token = "SAY";

    public string message;

    public CommandSay(string[] tokens)
    {
        player_id = Int32.Parse(tokens[0]);
        message = string.Join(" ", tokens, 2, tokens.Length - 2);   // Regex.Match(line, @"PNAME (.+)", RegexOptions.IgnoreCase).Groups[1].Value;
    }
    override public Matchstate Execute(Matchstate state)
    {
        state.messages.Add($"{state.players[player_id].name}: {message}");
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        state.messages.RemoveAt(state.messages.Count - 1);
        return state;
    }
}


public class CommandPTA : Command
{
    static public new string token = "PTA";

    private UnitType unitType;
    private UnitType targetType;
    private UnitType previousType;
    
    public CommandPTA(int player_id, UnitType unitType, UnitType targetUnitType)
    {
    }

    public CommandPTA(string[] tokens)
    {
        this.player_id = Int32.Parse(tokens[0]);
        this.unitType = StringToUnitType(tokens[2]);
        this.targetType = StringToUnitType(tokens[3]);
    }

    override public Matchstate Execute(Matchstate state)
    {
        previousType = state.players[player_id].pta[(int)unitType];
        state.players[player_id].pta[(int)unitType] = targetType;
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        state.players[player_id].pta[(int)unitType] = previousType;
        return state;
    }
}
