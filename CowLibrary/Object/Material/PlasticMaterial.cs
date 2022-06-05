namespace CowLibrary;

using System;
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
        if (Mathf.CosTheta(wo) < 0)
        {
            return Color.Black;
        }
        var f = reflection.Evaluate(in wo, in wi) + diffuse.Evaluate(in wo, in wi);
        return f * Color;
    }

    public Color Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var index = Math.Min(sample.X * 2, 1);
        var sampleMapped = new Vector2(Math.Min(sample.X * 2 - index, Const.OneMinusEpsilon), sample.Y);
       var  f = index == 0
            ? diffuse.Sample(in wo, in sampleMapped, out wi, out pdf)
            : reflection.Sample(in wo, in sampleMapped, out wi, out pdf);
        pdf += index == 0 ? reflection.Pdf(in wo, in wi) : diffuse.Pdf(in wo, in wi);
        pdf /= 2;
        f += index == 0 ? reflection.Evaluate(in wo, in wi) : diffuse.Evaluate(in wo, in wi);
        return f * Color;
    }

    public IMaterial Copy(int id)
    {
        return new PlasticMaterial(reflection, diffuse, Color, id);
    }
}
