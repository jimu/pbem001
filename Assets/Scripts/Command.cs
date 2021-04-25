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

public enum UnitType { Invalid, TB, LB, RB, JB, BB, BPCPI, BPCPS, BPCPN, MISSILE_AA, MISSILE_AG, MISSILE_AM };
public enum DeploymentType { Invalid, MapEdge, NearBPCP }

[CreateAssetMenu]
public class UnitTypeData : ScriptableObject
{
    public string code;
    public string strength;
    public string movementAllowance;
    public Sprite icon;
    public int quantity;
    public DeploymentType deploymentType;
    public int stackingLimit;

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
