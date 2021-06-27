using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField] HexGrid hexGrid;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeployUnit(Bopper.Unit unit)
    {
        DeployUnit(unit.id, unit.name, unit.player_id, unit.data, unit.coord, unit.layer);
    }

    public void DeployUnit(int id, string name, int player_id, UnitData data, int coord, int layer)
    {
        Debug.Log($"View.DeployUnit({id}, utype={data.unitType}, layer={layer})");
        Debug.Assert((int)data.unitType > 0, "DeployUnit fails because unittype is 0", data);

        // HexGrid.ColorCell(Vector3 position, Color color, UnitData data): cell.SetUnit(data);
        //   HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //   HexCell.SetUnit(UnitData data)
        HexCoordinates hcoord = HexCoordinates.FromRivets(coord);
        hexGrid.CreateUnitInCell(hcoord, id, data, player_id == 1, layer);
    }


    public void UndeployUnit(Bopper.Unit unit)
    {
        Debug.Log($"View.UndeployUnit({unit.id})"); //todo
    }

}
