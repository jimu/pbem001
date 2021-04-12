using UnityEngine;

/// <summary>
/// used to convert to a different coordinate system.
/// </summary>
/// <remarks>
///  Serializable so Unity can store it, which allows them to survive recompiles while in play mode.
///  Also, immutable by using public readonly properties.
///  \image html offset-diagram.png
///  </remarks>
[System.Serializable]
public class HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X { get { return x; }}
    public int Z { get { return z; }}

    // used to calculate cube-coordinates
    public int Y { get { return -X - Z; } }

    public HexCoordinates (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    /// <summary>
    /// Factory: creates a set of Axial Coordinates using regular offset coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// 
    /// @image html axial-diagram.png
    /// 
    /// <returns>HexCoordinates</returns>
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }
    /// <summary>
    /// Returns Cube-Coordinates in the form (x, y, z)
    /// </summary>
    /// <returns>string Cube-Coordinates</returns>
    /// @image html cube-diagram.png

    public override string ToString()
    {
        return $"({X.ToString()}, {Y.ToString()}, {Z.ToString()})";
    }

    /// <summary>
    /// Returns Axial-Coordinates in the form (x, z)
    /// </summary>
    /// <returns>string Axial-Coordinates</returns>
    public string ToString2()
    {
        return $"({X.ToString()}, {Z.ToString()})";
    }

    /// <summary>
    /// Returns Cube-Coordinates on three lines
    /// </summary>
    /// <returns>string Cub-Coordinates</returns>
    public string ToStringOnSeparateLines()
    {
        return $"{X.ToString()}\n{Y.ToString()}\n{Z.ToString()}";
    }
    /// <summary>
    /// Returns Axial-Coordinates on two lines
    /// </summary>
    /// <returns>string Axial-Coordinates</returns>
    public string ToStringOnSeparateLines2()
    {
        return $"{X.ToString()}\n{Z.ToString()}";
    }
}
