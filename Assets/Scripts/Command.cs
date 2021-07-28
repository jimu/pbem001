using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Text.RegularExpressions;
using Bopper.View;

// Commands
// deploy <type> <coord>
// pta <type> <type>
// say <message>




abstract public class Command
{
    static public string token;
    static public Bopper.View.Unity.GameView view;
    public int player_id;

    public long id; // for OSA

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

/// <summary>
/// Creates Commands objects from text command lines
/// </summary>
/// 		static string deployRegex = @"^\s*DEPLOY\s+(LB|RB|JB|BB|TB|BCPC)\s+(?:in)?\s*(?:hex)?\s*(\d+)";
///         static string moveRegex = @"^\s*MOVE\s+(LB|RB|JB|BB|TB)-?(\d+)\s+(?:to)?\s*(?:hex)?\s*(\d+)";
///         static string ptaRegex = @"^\s*PTA\s+(LB|RB|JB|BB|TB)\s+(LB|RB|JB|BB|TB)";

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
            cmd == CommandMove.token ? new CommandMove(tokens) :
            cmd == CommandPhase.token ? new CommandPhase(tokens) :
            cmd == CommandShortname.token ? new CommandShortname(tokens) :
            (Command)null;
    }

    public static List<Command> MakeMany(string commandSetText)
    {
        var commands = new List<Command>();
        using (var sr = new StringReader(commandSetText))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Command command = Make(line);
                if (command != null)
                    commands.Add(command);
                else
                    Debug.Log($"Invalid command: {line}");
            }
        }
        return commands;
    }
}

// Not a command, rather a way of including a status message in the Matchstate
public class CommandStatus
{
    public enum Type { None, Deploy, Phase };

    public Type type = Type.None;
    public long parameter1;

    public CommandStatus(Type type = Type.None, long parameter1 = 0)
    {
        this.type = type;
        this.parameter1 = parameter1;
    }
}


public class CommandPName : Command
//########################################## PNAME ################################################
{

    static public new string token = "NAME";
    private string name;
    private string previousName;

    [Obsolete]
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
        return $"Player name set to \"{name}\"";
    }
}
public class CommandDeploy : Command
//########################################## DEPLOY ################################################
{
    static public new string token = "DEPLOY";
    public UnitType unitType;
    public int coord;
    public int layer;
    public int index;
    public Bopper.Unit unit;
    public string name;

    public CommandDeploy(int player_id, UnitType unitType, int hex, int layer = 0, string name = "")
    {
        this.player_id = player_id;
        this.unitType  = unitType;
        this.coord     = hex;
        this.layer     = layer;
        this.name      = name;
    }

    /// <summary>
    /// Construct a DEPLOY command from tokens
    /// </summary>
    /// <param name="tokens">[pid, DEPLOY, unitType, coord, [layer]]</param>
    public CommandDeploy(string[] tokens)
    {
        player_id = Int32.Parse(tokens[0]);
        unitType = StringToUnitType(tokens[2]);
        //coord = Int32.Parse(tokens[3]);
        //layer = tokens.Length > 4 ? Int32.Parse(tokens[4]) : 0;
        name = tokens.Length > 4 ? tokens[4] : "";
        ParseCoord(tokens[3], out coord, out layer);
        Debug.Log($"CommandDeploy: coord:{coord} layer:{layer} name:{name}");
    }

    static public bool ParseCoord(string token, out int coord, out int layer)
    {
        int dotAt = token.IndexOf(".");
        coord = dotAt > 0 ? Int32.Parse(token.Substring(0,dotAt)) : Int32.Parse(token);
        layer = dotAt > 0 ? Int32.Parse(token.Substring(dotAt + 1)) : 0;
        return true;
    }

    /// <summary>
    /// Execute DEPLOY command, adding unit to the state and calling view.DeployUnit.
    /// Note that Execute and Undo are called just by clicking lines in the log. Each "command" log line is tied to a command.  The command persists with the log line. When the
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    override public Matchstate Execute(Matchstate state)
    {
        Debug.Log($"CommandDeploy({ToString()})");
        unit = new Bopper.Unit(unitType, player_id, coord, layer, name);
        if (unit == null)
            throw new Exception($"UNIT IS NULL");

        Debug.Assert(unit.data != null, "UNIT DATA IS NULL");
        if (unit.data == null)
            throw new Exception($"unit.name={unit.name}, unit.data={unit.data.name}: UNIT DATA IS NULL");
        
        Debug.Assert(unit.data.counterPrefab != null, $"unit.data.name={unit.data.name} does not have a counterPrefab");
        if (unit.data == null || unit.data.counterPrefab == null)
            throw new Exception($"unit.data.name={unit.data.name} str={unit.data.strength} does not have a counterPrefab");
        Debug.Log($"{unit} utype={unit.data.unitType}");
        //index = state.units.Count;
        state.units.Add(unit);
        //state.SetStatus(CommandStatus.Type.Deploy, unit.id);
        //state.SetStatus($"DEPLOY {unit.name} in hex {unit.coord}");
        //view.Display(this);                                               // we can have only one view. Right now it must be Bopper.View.Unity.GameView. Views need to know about Commands
        ViewMaster.deployListeners?.Invoke(unit, ToString());               // we don't need to know about anything about any view(s). Views don't need to know anything about Commands
        return state;
    }

#if false  // needs to know about ViewMaster. Views don't need to know about Commands, and Commands don't need to know about views.  Both just need to know about interface
    override public Matchstate Execute(Matchstate state)
    {
        unit = new Bopper.Unit(unitType, player_id, coord, layer);
        state.units.Add(unit);
        ViewMaster.deployListeners?.Invoke(unit, ToString());               // we don't need to know about anything about any view(s). Views don't need to know anything about Commands
        return state;
    }
#endif



    override public Matchstate Undo(Matchstate state)
    {
        ViewMaster.undeployListeners?.Invoke(unit);    // we don't need to know about anything about any view(s). Views don't need to know anything about Commands
        //view.DisplayUndeploy(unit);
        //state.units.RemoveAt(index);
        state.units.Remove(unit);
        return state;
    }

    public override string ToString()
    {
        return layer > 0 ?
            $"Deploy {unitType} in factory" :
            $"Deploy {unitType} in {coord}";
    }
}
public class CommandMove : Command
//########################################## MOVE ################################################
{
    static public new string token = "MOVE";
    public string unit_name;
    public int coord;
    public int layer;
    public int index;
    public Bopper.Unit unit;

    public int prevCoord;
    public int prevLayer;

    public CommandMove(int player_id, string unit_name, int hex, int layer = 0)
    {
        this.player_id = player_id;
        this.unit_name = unit_name;
        this.coord = hex;
        this.layer = layer;
    }

    /// <summary>
    /// Construct a MOVE command from tokens
    /// </summary>
    /// <param name="tokens">[pid, DEPLOY, id, coord, [layer]]</param>
    public CommandMove(string[] tokens)
    {
        player_id = Int32.Parse(tokens[0]);
        unit_name = tokens[2];
        coord = Int32.Parse(tokens[3]);
        layer = tokens.Length > 4 ? Int32.Parse(tokens[4]) : 0;
    }

    /// <summary>
    /// Execute DEPLOY command, adding unit to the state and calling view.DeployUnit.
    /// Note that Execute and Undo are called just by clicking lines in the log. Each "command" log line is tied to a command.  The command persists with the log line. When the
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    override public Matchstate Execute(Matchstate state)
    {
        Debug.Log(ToString());
        unit = state.findUnit(player_id, unit_name);

        Debug.Log($"Move: unit={unit}");
        prevCoord = unit.coord;
        prevLayer = unit.layer;

        prevCoord = unit.coord;
        unit.coord = coord;
        ViewMaster.moveListeners?.Invoke(unit, prevCoord, prevLayer, ToString());    // we don't need to know about anything about any view(s). Views don't need to know anything about Commands

        return state;
    }


    override public Matchstate Undo(Matchstate state)
    {
        //view.UndeployUnit(unit);
        //state.units.RemoveAt(index);

        unit.coord = prevCoord;
        unit.layer = prevLayer;
        ViewMaster.unmoveListeners?.Invoke(unit, coord, layer);
        return state;
    }

    public override string ToString()
    {
        return layer > 0 ?
            $"Move {unit_name} to factory" :
            $"Move {unit_name} to {coord}";
    }
}
public class CommandPhase : Command
//########################################## PHASE ################################################
{
    static public new string token = "PHASE";
    public string name;

    public CommandPhase(int player_id, string name)
    {
        this.player_id = player_id;
        this.name = name;
    }

    /// <summary>
    /// Construct a PHASE command from tokens
    /// </summary>
    /// <param name="tokens">[pid, PHASE, name]</param>
    public CommandPhase(string[] tokens)
    {
        player_id = Int32.Parse(tokens[0]);
        name = string.Join(" ", tokens, 2, tokens.Length - 2);
    }

    /// <summary>
    /// Execute DEPLOY command, adding unit to the state and calling view.DeployUnit.
    /// Note that Execute and Undo are called just by clicking lines in the log. Each "command" log line is tied to a command.  The command persists with the log line. When the
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    override public Matchstate Execute(Matchstate state)
    {
        Debug.Log(ToString());
        ViewMaster.phaseListeners?.Invoke(name);               // we don't need to know about anything about any view(s). Views don't need to know anything about Commands

        return state;
    }



    override public Matchstate Undo(Matchstate state)
    {
        Debug.Log("Revert " + ToString());
        return state;
    }

    public override string ToString()
    {
        return $"Phase {name}";
    }
}
public class CommandSay : Command
//########################################## SAY ################################################
{
    static public new string token = "SAY";

    public string message;


    public CommandSay(int player_id, string message)
    {
        this.player_id = player_id;
        this.message = message;
    }
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
    public override string ToString()
    {
        return message;
    }
}
public class CommandPTA : Command
//########################################## PTA ################################################
{
    static public new string token = "PTA";

    private UnitType unitType;
    private UnitType targetType;
    private UnitType previousType;

    public CommandPTA(int player_id, UnitType unitType, UnitType targetUnitType)
    {
        this.player_id = player_id;
        this.unitType = unitType;
        this.targetType = targetUnitType;
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
    public override string ToString()
    {
        return $"{unitType} Programmed to Attack {targetType}";
    }

}
public class CommandAttack
{

}
public class CommandAttackResult
{

}

public class CommandShortname : Command
//########################################## SHORT ################################################

{
    static public new string token = "SHORTNAME";

    private readonly string shortname;
    private string previousShortname;

    [Obsolete]
    public CommandShortname(int player_id, string shortname)
    {
        this.player_id = player_id;
        this.shortname = shortname;
    }

    public CommandShortname(string[] tokens)
    {
        this.player_id = Int32.Parse(tokens[0]);
        this.shortname = tokens[2].ToUpper();
    }

    override public Matchstate Execute(Matchstate state)
    {
        previousShortname = state.players[player_id].shortname;
        state.players[player_id].shortname = shortname; ;
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        state.players[player_id].shortname = previousShortname;

        return state;
    }
    public override string ToString()
    {
        return $"Short name set to \"{shortname}\"";
    }

}
public class CommandEcho : Command
{
    static public new string token = "ECHO";

    private string message;

    public CommandEcho(int player_id, string message)
    {
        this.player_id = player_id;
        this.message = message;
    }

    public CommandEcho(string[] tokens)
    {
        this.player_id = Int32.Parse(tokens[0]);
        this.message = string.Join(" ", tokens, 2, tokens.Length - 2);   // Regex.Match(line, @"PNAME (.+)", RegexOptions.IgnoreCase).Groups[1].Value;
    }

    override public Matchstate Execute(Matchstate state)
    {
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        return state;
    }
    public override string ToString()
    {
        return $"ECHO {message}";
    }
}
public class CommandStatusMessage : Command
{
    static public new string token = "ECHO";

    private string message;

    public CommandStatusMessage(int player_id, string message)
    {
        this.player_id = player_id;
        this.message = message;
    }

    public CommandStatusMessage(string[] tokens)
    {
        this.player_id = Int32.Parse(tokens[0]);
        this.message = string.Join(" ", tokens, 2, tokens.Length - 2);   // Regex.Match(line, @"PNAME (.+)", RegexOptions.IgnoreCase).Groups[1].Value;
    }

    override public Matchstate Execute(Matchstate state)
    {
        return state;
    }

    override public Matchstate Undo(Matchstate state)
    {
        return state;
    }
    public override string ToString()
    {
        return $"ECHO {message}";
    }
}


