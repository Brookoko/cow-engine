namespace CowLibrary.Lights.Models;

using System.Numerics;

public readonly struct EnvironmentLightModel : ILightModel
{
    private readonly Color color;

    public int Id { get; }

    public EnvironmentLightModel(Color color, int id)
    {
        this.color = color;
        Id = id;
    }

    public ShadingInfo GetShadingInfo(in RayHit rayHit, in Matrix4x4 localToWorldMatrix, in Vector2 sample)
    {
        var wi = Mathf.CosineSampleHemisphere(sample);
        wi = Mathf.ToWorld(in rayHit.normal, in wi);
        return new ShadingInfo(wi, color, float.PositiveInfinity);
    }

    public Color Sample(in Vector3 wi, in Vector2 sample)
    {
        return color;
    }
}
