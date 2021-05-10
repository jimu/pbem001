using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data\\units\\New Unit", menuName = "Unit")]
public class UnitData : ScriptableObject
{
    public int id;
    public string code;
    public GameObject counterPrefab;
    public GameObject trayPrefab;
    public Sprite icon;
    public int strength;
    public int movementAllowance;
    public int quantity;
    public DeploymentType deploymentType;
    public int stackingLimit;
    public UnitType unitType;
}


public enum UnitType { Invalid, TB, LB, RB, JB, BB, BPCPI, BPCPS, BPCPN, MISSILE_AA, MISSILE_AG, MISSILE_AM };
public enum DeploymentType { Invalid, MapEdge, NearBPCP }