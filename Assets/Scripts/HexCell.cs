using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// A single hex
/// </summary>
public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;

    public Color color;
    public List<Unit> units = new List<Unit>();



    public void SetUnit(UnitData data)
    {
        int count = 0;
        const int MAX_UNITS = 2;

        foreach(Unit u in units)
        {
            if (u.data == data)
            {
                count++;
            }
            else
            {
                // new unit: delete existing units and add new
                DeleteUnits();
                AddUnit(data);
                return;
            }
        }

        Debug.Log($"SetUnit (count={count})");

        if (count < MAX_UNITS)
            AddUnit(data, count);
        else
            DeleteUnits();
    }



    void DeleteUnits()
    {
        //Debug.Log($"DeleteUnits()");
        foreach (Unit u in units)
            DestroyImmediate(u.gameObject);
        units.Clear();
    }

    void AddUnit(UnitData data, int stackHeight = 0)
    {
        //Debug.Log($"AddUnit (code={data.code})");
        Vector3 pos = transform.position;
        pos.y += stackHeight * 2f + 0.5f;
        pos.x -= stackHeight * 2f + 0.5f;
        pos.z += stackHeight * 2f + 0.5f;
        Unit counter = Instantiate(data.counterPrefab, pos, data.counterPrefab.transform.rotation);
        units.Add(counter);
    }
}
