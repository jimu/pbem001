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

    public float label_offset_x = 6.5f;

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

    public Color defaultColor = new Color32(0x76, 0x5D, 0x46, 0xFF); // 1, 0.5f, 0.5f); // Color.white; // 765D46
    public Color highlightColor = new Color32(0x86, 0x6D, 0x56, 0xFF); // 1, 0.5f, 0.5f); // Color.white; // 765D46

    /// <summary>
    /// Depricated - color used when hex was touched
    /// </summary>
    // public Color depricated_touchedColor = Color.magenta;

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

        string rivetString = cell.coordinates.ToRivetsString();
        bool inRangeOfBCPC = rivetString == "1103" || rivetString == "1104";
        HexCoordinates bcpc = HexCoordinates.FromRivets(11, 5);
        int distance = bcpc.Distance(cell.coordinates);
        // Debug.Log($"{bcpc} ({bcpc.ToRivetsString()}) to {cell.coordinates} ({cell.coordinates.ToRivetsString()}): {distance}");
        inRangeOfBCPC = distance <= 5; // todo
        cell.color = inRangeOfBCPC ? highlightColor : defaultColor; // new Color(1f, 0.5f, 0.5f);// Color.green;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x + label_offset_x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();  // $"{x}\n{z}";
        label.text = cell.coordinates.ToRivetsString();
    }


    public void ColorCell(Vector3 position, Color color, UnitData data)
    {
        position = transform.InverseTransformPoint(position);
        // Use HexCoordinates to convert mouse position to hex-coordinates
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //Debug.Log($"touched at {position} ({coordinates.ToString()}");

        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        // cell.color = color;
        hexMesh.Triangulate(cells);
        cell.SetUnit(data);
        FindObjectOfType<PointTracker>()?.UpdatePoints();
    }

}
