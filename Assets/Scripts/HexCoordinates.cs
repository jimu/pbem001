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
    /// Returns a new HexCoordinates object from a grid plane position
    /// </summary>
    /// <param name="position">point on grid</param>
    /// <returns></returns>
    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            // Debug.LogWarning("rounding error!");
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
                iX = -iY - iZ;
            else if (dZ > dY)
                iZ = -iX - iY;
        }

        return new HexCoordinates(iX, iZ);
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


    public string ToRivetsString()
    {
        return $"{Z+1:D2}{X+8:D2}";
    }
    public static HexCoordinates FromRivets(int x, int z)
    {
        return new HexCoordinates(z-8, x-1);
    }

    public static HexCoordinates FromRivets(int coord)
    {
        return new HexCoordinates((coord % 100) - 8, (coord / 100) - 1);
    }

    public int Distance(HexCoordinates other)
    {
        int dx = Mathf.Abs(X - other.X);
        int dy = Mathf.Abs(Y - other.Y);
        int dz = Mathf.Abs(Z - other.Z);
        return (dx + dy + dz) / 2;
    }
}
