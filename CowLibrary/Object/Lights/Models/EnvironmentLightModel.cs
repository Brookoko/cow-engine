namespace CowLibrary.Lights.Models;

using System.Numerics;

public readonly struct EnvironmentLightModel : ILightModel
{
    private readonly Color color;

    public EnvironmentLightModel(Color color)
    {
        this.color = color;
    }

    public ShadingInfo GetShadingInfo(in RayHit rayHit, in Matrix4x4 localToWorldMatrix, in Vector2 sample)
    {
        return new ShadingInfo(Mathf.CosineSampleHemisphere(rayHit.normal, sample), color, float.PositiveInfinity);
    }

    public Color Sample(in Vector3 wi, in Vector2 sample)
    {
        return color;
    }
}
