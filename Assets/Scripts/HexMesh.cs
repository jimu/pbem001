using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a single mesh to render the entire grid. 
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertices;

    /// <summary>
    /// list of 3-tuples containing vertices indexes specifying triangles (0,1,2) is first triangle (3,4,5) is second triange, ...
    /// </summary>
    List<int> triangles;


    MeshCollider meshCollider;

    List<Color> colors;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    /// <summary>
    /// Fills verticies and triangle structures with hex cells
    /// </summary>
    /// <remarks>
    /// Each element in cells will generate 6 triangles and 18 verticies
    /// </remarks>
    /// <param name="cells"></param>
    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        for (int i = 0; i < cells.Length; i++)
            Triangulate(cells[i]);

        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();

        meshCollider.sharedMesh = hexMesh;
    }

    /// <summary>
    /// Fills verticies and triangle structures with a hex cell
    /// </summary>
    /// <remarks>Generates 6 triangles and 18 verticies</remarks>
    /// <param name="cell"></param>
    void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;

        //AddTriangle(center, center + HexMetrics.corners[0], center + HexMetrics.corners[1]);

        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i + 1]);
            AddTriangleColor(cell.color);
        }
    }

    /// <summary>
    /// Adds a triangle with the specified verticies
    /// </summary>
    /// <param name="v1">Center vertice</param>
    /// <param name="v2">First outer vertice</param>
    /// <param name="v3">Last outer vertice</param>
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }   

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
}
