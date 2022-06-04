namespace CowLibrary;

using System.Numerics;
using Models.Microfacet;

public readonly struct PlasticMaterial : IMaterial
{
    public Color Color { get; }
    public int Id { get; }

    private readonly PlasticBrdf reflection;
    private readonly LambertianBrdf diffuse;

    public PlasticMaterial(Color color, float r, float rough, int id)
    {
        var fresnel = new DielectricFresnel(1.5f, 1);
        rough = Mathf.RoughnessToAlpha(rough);
        var distribution = new TrowbridgeReitzDistribution(rough, rough);
        var brdf = new PlasticBrdf(r, fresnel, distribution);
        var diffuse = new LambertianBrdf(r);
        this = new PlasticMaterial(brdf, diffuse, color, id);
    }

    private PlasticMaterial(PlasticBrdf reflection, LambertianBrdf diffuse, Color color, int id)
    {
        Color = color;
        Id = id;
        this.reflection = reflection;
        this.diffuse = diffuse;
    }

    public Color GetColor(in Vector3 wo, in Vector3 wi)
    {
        var f = reflection.Evaluate(in wo, in wi) + diffuse.Evaluate(in wo, in wi);
        return f * Color;
    }

    public Color Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var index = (int)(sample.X * 2);
        if (index == 0)
        {
            return diffuse.Sample(in wo, in sample, out wi, out pdf) * Color;

        }
        return reflection.Sample(in wo, in sample, out wi, out pdf) * Color;
    }

    public IMaterial Copy(int id)
    {
        return new PlasticMaterial(reflection, diffuse, Color, id);
    }
}
