using UnityEngine;

/// <summary>
/// Provides coordinates of the vertices of a hexagon centered at (0,0).
/// Positioned along the X-Z axis, with pointy corner along the Z-Axis.
/// </summary>
public static class HexMetrics
{
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public const float scale = 0.98f;

    /// <summary>
    /// Coordinates of a hexagon with center at (0,0) and given outerRadius.
    /// 
    /// Hex on XZ plane, pointy side up
    /// </summary>

    public static Vector3[] corners_original =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f,  0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f,  0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius),

    };

    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius * scale),
        new Vector3(innerRadius * scale, 0f,  0.5f * outerRadius * scale),
        new Vector3(innerRadius * scale, 0f, -0.5f * outerRadius * scale),
        new Vector3(0f, 0f, -outerRadius * scale),
        new Vector3(-innerRadius * scale, 0f, -0.5f * outerRadius * scale),
        new Vector3(-innerRadius * scale, 0f,  0.5f * outerRadius * scale),
        new Vector3(0f, 0f, outerRadius * scale),

    };
}
