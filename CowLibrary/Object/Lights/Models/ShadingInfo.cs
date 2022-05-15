namespace CowLibrary.Lights.Models;

using System.Numerics;

public readonly struct ShadingInfo
{
    public readonly Vector3 direction;
    public readonly Color color;
    public readonly float distance;

    public ShadingInfo(Vector3 direction, Color color, float distance)
    {
        this.direction = direction;
        this.color = color;
        this.distance = distance;
    }
}
