namespace CowLibrary;

using System.Numerics;
using Models.Microfacet;

public readonly struct BlendMaterial : IMaterial
{
    public Color Color { get; }
    public int Id { get; }

    private readonly FresnelBlend brdf;

    public BlendMaterial(Color diffuse, Color specular, float rough, int id)
    {
        var distribution = new TrowbridgeReitzDistribution(rough, rough);
        var brdf = new FresnelBlend(diffuse, specular, distribution);
        this = new BlendMaterial(diffuse, brdf, id);
    }

    private BlendMaterial(Color color, FresnelBlend brdf, int id)
    {
        Color = color;
        Id = id;
        this.brdf = brdf;
    }

    public Color GetColor(in Vector3 wo, in Vector3 wi, in Vector3 normal)
    {
        return brdf.Evaluate(in wo, in wi, in normal);
    }

    public Color Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        return brdf.Sample(in normal, in wo, in sample, out wi, out pdf);
    }

    public IMaterial Copy(int id)
    {
        return new BlendMaterial(Color, brdf, id);
    }
}
