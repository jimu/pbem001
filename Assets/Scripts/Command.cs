using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

// Commands
// deploy <type> <coord>
// pta <type> <type>
// say <message>



public class Command
{
    virtual public void execute()
    {

    }

    public void undo()
    {

    }
}




public class Unit
{
    UnitType unitType;
    int id;
    int hex;
}

public class CommandDeploy : Command
{
    public CommandDeploy(UnitType unitType, int hex)
    {

    }
}
