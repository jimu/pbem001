using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Bopper's Unity UI
/// </summary>
/// 
namespace Bopper.View.Unity
{
    /// <summary>
    /// A HexCell is a single graphical hex on the map responsible for it's HexCoordinates and a list of <see cref="Unit"/>s in the hex.
    /// </summary>
    /// <remarks>Responsible for knowing it's HexCoordinates and list of <c>Units</c> in the hex</remarks>
    public class HexCell : MonoBehaviour
    {
        public HexCoordinates coordinates;

        public Color color;
        public List<Unit> units = new List<Unit>();



        public void SetUnit(UnitData data, bool altColor = false)
        {
            int count = 0;
            const int MAX_UNITS = 2;

            foreach (Unit u in units)
            {
                if (u.data == data)
                {
                    count++;
                }
                else
                {
                    // new unit: delete existing units and add new
                    DeleteUnits();
                    AddUnit(data, altColor);
                    return;
                }
            }

            //Debug.Log($"SetUnit (count={count})");

            if (count < MAX_UNITS)
                AddUnit(data, altColor, count);
            else
                DeleteUnits();
        }
        public void ShowContents()
        {
            string output = $"Contents of {coordinates}:\n";
            foreach (var unit in units)
            {
                output += $" - {unit.id} {unit.name}\n";
            }
            Debug.Log(output);
        }



        public Unit CreateStackableUnit(int id, UnitData data, bool altColor = false, int layer = 0)
        {
            Debug.Log($"CreateStackableUnit({id}, {data.name}, {altColor}, {layer}) data.counterPrefab={data.counterPrefab} this={this.name}");
            Unit counter = Instantiate(data.counterPrefab, transform.position, data.counterPrefab.transform.rotation);
            counter.id = id;
            if (altColor)
                counter.MakeRed();
            if (layer > 0)
                units.Insert(0, counter);
            else
                units.Add(counter);

            RepositionStack();

            return counter;
        }
        public Unit AddStackableUnit(Unit counter, int layer = 0)
        {
            Debug.Log($"AddStackableUnit({counter.id}, {counter.name}");

            if (layer > 0)
                units.Insert(0, counter);
            else
                units.Add(counter);

            DumpUnits(units, $"AddStackableUnit({counter.name}, L{layer}): ");

            RepositionStack();

            return counter;
        }

        void DumpUnits(List<Unit> units, string message)
        {
            foreach (var unit in units)
                message += $" {unit.name}";
            Debug.Log(message);
        }

        public void DestroyStackableUnit(int id)
        {
            foreach (var unit in units)
                if (unit.id == id)
                {
                    units.Remove(unit);
                    Destroy(unit.gameObject);
                    RepositionStack();
                    return;
                }
        }

        public Unit RemoveStackableUnit(int id)
        {
            foreach (Unit unit in units)
            {
                //Debug.Log($"RemoveStackableUnit({id}) checking against {unit.id}");
                if (unit.id == id)
                {
                    units.Remove(unit);
                    RepositionStack();
                    return unit;
                }
            }

            throw new System.Exception($"RemoveStackableUnit: cannot find unit id={id}");
            return null;
        }

        bool IsFactoryStack()
        {
            return units.Count > 2 || units.Count == 2 && (units[0].data.stackingLimit == 0 || units[1].data.stackingLimit == 0);
        }

        void RepositionStack()
        {
            // if there are more than 2 units, we are in a BCPC and we want to show unit names to right
            float dz = IsFactoryStack() ? -3f : 2f;
            Debug.Log($"RepositionStack: {IsFactoryStack()}");

            Vector3 pos = transform.position;
            pos.y += 0.5f;
            pos.x -= 0.5f;
            pos.z += 0.5f;

            foreach (Unit unit in units)
            {
                Debug.Log($"Repositioning unit {unit}: ({pos})");
                unit.transform.position = pos;
                pos.y += 2f;
                pos.x -= 2f;
                pos.z += dz;
            }
        }

        void DeleteUnits()
        {
            //Debug.Log($"DeleteUnits()");
            foreach (Unit u in units)
                DestroyImmediate(u.gameObject);
            units.Clear();
        }

        void AddUnit(UnitData data, bool altColor = false, int stackHeight = 0)
        {
            //Debug.Log($"AddUnit (code={data.code})");
            Vector3 pos = transform.position;
            pos.y += stackHeight * 2f + 0.5f;
            pos.x -= stackHeight * 2f + 0.5f;
            pos.z += stackHeight * 2f + 0.5f;
            Unit counter = Instantiate(data.counterPrefab, pos, data.counterPrefab.transform.rotation);
            if (altColor)
                counter.MakeRed();
            units.Add(counter);
        }
    }
}