using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public HexGrid hexGrid;
    public Color[] colors;
    public UnitData[] units;
    private Color activeColor;
    private UnitData activeUnit;

    private void Awake()
    {
        SelectColor(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
 //           hexGrid.ColorCell(hit.point, activeColor, activeUnit);
            hexGrid.ShowContents(hit.point);
        }
    }

    public void SelectColor(int colorIndex)
    {
        activeColor = colors[colorIndex];
        activeUnit = units[colorIndex];
    }
}
