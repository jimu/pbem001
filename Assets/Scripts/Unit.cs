using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData data;
    public int id;

    public Unit(int id, UnitData data)
    {
        this.id = id;
        this.data = data;
    }

    private void OnMouseDown()
    {
        Debug.Log("Unit {this} Clicked");
    }

    public void MakeRed()
    {
        var mr = GetComponentInChildren<MeshRenderer>();
        mr.materials = new Material[] { GameManager.instance.redMaterial };
        foreach (var text in GetComponentsInChildren<UnityEngine.UI.Text>())
            text.color = Color.white;
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
            sprite.color = Color.white;
    }
}

namespace Bopper
{
    [Serializable]
    public class Unit
    {
        public int id;
        public int coord;
        public int player_id;
        public string name;
        public UnitData data;
        public int layer;

        static List<Unit> units = new List<Unit>();

        public Unit(UnitType utype, int player_id, int coord, int layer)
        {
            Debug.Assert(utype > 0, "UnitType must be > 0");
            this.player_id = player_id;
            this.coord = coord;
            this.layer = layer;
            SetData(utype);

            id = units.Count;
            units.Add(this);

            name = $"{utype}-{id}";
        }

        private void SetData(UnitType utype)
        {
            data = GameManager.instance.gameData.unitData[(int)utype];
            Debug.Log($"Setting data to {utype}({(int)utype}): [{data.name}, {data.unitType}]");
        }


        public override string ToString()
        {
            return $"[{name} owner:{player_id} coord:{coord}]";
        }
    }

}