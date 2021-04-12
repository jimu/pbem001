using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Needed to access UnityEngin.UI.Text

/// <summary>
///   A grid of hexes
/// </summary>
public class HexGrid : MonoBehaviour
{
    /// <summary>
    /// Number of HexCells in the grid along the x-axis
    /// </summary>
    public int width = 6;

    /// <summary>
    /// Number of HexCells in the grid along the z-axis
    /// </summary>
    public int height = 6;

    /// <summary>
    /// The HexCell prefab we used to instantiate the cells in this grid
    /// </summary>
    public HexCell cellPrefab;

    /// <summary>
    /// The Coordinate label prefab used to display coordinate text
    /// </summary>
    public Text cellLabelPrefab;

    /// <summary>
    /// HexMesh object used to hold the triangle geometry of the hexagon
    /// </summary>
    public HexMesh hexMesh;

    /// <summary>
    /// Holds HexCell objects belonging to this HexGrid
    /// </summary>
    HexCell[] cells;

    /// <summary>
    /// Canvas with text objects for coordinate labels
    /// </summary>
    Canvas gridCanvas;

    /// <summary>
    /// Creates all the HexCells needed by the HexGrid
    /// </summary>
    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                CreateCell(x, z, i++);
    }

    /// <summary>
    /// Creates hex geometry for all cells after hexMesh has awoken
    /// </summary>
    private void Start()
    {
        hexMesh.Triangulate(cells);
    }
    /// <summary>
    ///   Instantiates a HexCell in cells[i] at x*10, z*10. Parents the HexCell GameObject to this HexGrid
    /// </summary>
    /// <param name="x">x-grid position</param>
    /// <param name="z">z-grid position</param>
    /// <param name="i">index into cells[]</param>
    void CreateCell (int x, int z, int i)
    {
        Vector3 position;
        //position.x = x * 10f;     // grid positioning
        //position.z = z * 10f;
        
        position.x = (x + z / 2f - z / 2 ) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();  // $"{x}\n{z}";
    }
}
