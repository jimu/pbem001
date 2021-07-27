using UnityEngine;

namespace Bopper.View.Unity
{
    public class Unit : MonoBehaviour
    {
        public UnitData data;
        public int id;

        public Unit(int id, UnitData data)
        {
            Debug.Log($"Unit CTOR has been called with ID {id} and data {data}");
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
}
