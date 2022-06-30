namespace CowLibrary.Lights.Models;

using System;
using System.Numerics;

public readonly struct PointLightModel : ILightModel
{
    private readonly Color color;

    public int Id { get; }

    public PointLightModel(Color color, int id)
    {
        this.color = color;
        Id = id;
    }

    public ShadingInfo GetShadingInfo(in RayHit rayHit, in Matrix4x4 localToWorldMatrix, in Vector2 sample)
    {
        var direction = localToWorldMatrix.ExtractTranslation() - rayHit.point;
        var sqrtDistance = direction.LengthSquared();
        var distance = (float)Math.Sqrt(sqrtDistance);
        return new ShadingInfo(direction / distance, color / (4 * Math.PI * sqrtDistance), distance);
    }

    public Color Sample(in Vector3 wi, in Vector2 sample)
    {
        return Color.Black;
    }
}
