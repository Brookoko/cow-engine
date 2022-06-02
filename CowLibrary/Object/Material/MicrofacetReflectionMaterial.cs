namespace CowLibrary;

using System.Numerics;
using Models.Microfacet;

public readonly struct MicrofacetReflectionMaterial : IMaterial
{
    public Color Color { get; }
    public int Id { get; }

    private readonly MicrofacetReflection brdf;

    public MicrofacetReflectionMaterial(Color color, float r, float eta, float rough, int id)
    {
        var fresnel = new DielectricFresnel(1, eta);
        var distribution = new TrowbridgeReitzDistribution(rough, rough);
        var brdf = new MicrofacetReflection(r, fresnel, distribution);
        this = new MicrofacetReflectionMaterial(brdf, color, id);
    }

    private MicrofacetReflectionMaterial(MicrofacetReflection brdf, Color color, int id)
    {
        Color = color;
        this.brdf = brdf;
        Id = id;
    }

    public Color GetColor(in Vector3 wo, in Vector3 wi, in Vector3 normal)
    {
        return brdf.Evaluate(in wo, in wi, in normal) * Color;
    }

    public Color Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        return brdf.Sample(in normal, in wo, in sample, out wi, out pdf) * Color;
    }

    public IMaterial Copy(int id)
    {
        return new MicrofacetReflectionMaterial(brdf, Color, id);
    }
}
