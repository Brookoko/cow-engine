namespace CowLibrary;

using System.Numerics;
using Models.Microfacet;

public readonly struct MetalMaterial : IMaterial
{
    public Color Color { get; }
    public int Id { get; }

    private readonly MetalBrdf brdf;

    public MetalMaterial(Color color, float r, float eta, float k, float rough, int id)
    {
        var fresnel = new ConductorFresnel(1, eta, k);
        rough = Mathf.RoughnessToAlpha(rough);
        var distribution = new TrowbridgeReitzDistribution(rough, rough);
        var brdf = new MetalBrdf(r, fresnel, distribution);
        this = new MetalMaterial(brdf, color, id);
    }

    private MetalMaterial(MetalBrdf brdf, Color color, int id)
    {
        Color = color;
        this.brdf = brdf;
        Id = id;
    }

    public Color GetColor(in Vector3 wo, in Vector3 wi)
    {
        return brdf.Evaluate(in wo, in wi) * Color;
    }

    public Color Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        return brdf.Sample(in wo, in sample, out wi, out pdf) * Color;
    }

    public IMaterial Copy(int id)
    {
        return new MetalMaterial(brdf, Color, id);
    }
}
