namespace CowLibrary;

using System;
using System.Numerics;
using Models.Microfacet;

public readonly struct FresnelBlend
{
    private readonly Color diffuse;
    private readonly Color specular;
    private readonly TrowbridgeReitzDistribution distribution;

    public FresnelBlend(Color diffuse, Color specular, TrowbridgeReitzDistribution distribution)
    {
        this.diffuse = diffuse;
        this.specular = specular;
        this.distribution = distribution;
    }

    public Color Evaluate(in Vector3 wo, in Vector3 wi)
    {
        var diffuseF = (28 / (23 * Const.Pi)) * diffuse * (Color.White - specular) *
                       (1 - Mathf.Pow5(1 - 0.5f * Mathf.AbsCosTheta(wi))) *
                       (1 - Mathf.Pow5(1 - 0.5f * Mathf.AbsCosTheta(wo)));
        var wh = wi + wo;
        if (wh.X == 0 && wh.Y == 0 && wh.Z == 0)
        {
            return Color.Black;
        }
        wh = wh.Normalize();
        var cos = Math.Max(Mathf.AbsCosTheta(wi), Mathf.AbsCosTheta(wo));
        var specularF = distribution.D(wh) / (4 * Mathf.AbsDot(wi, wh) * cos) *
                        SchlickFresnel(Vector3.Dot(wi, wh));
        return diffuseF + specularF;
    }

    public Color Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var s = sample;
        if (s.X < 0.5f)
        {
            s.X = Math.Min(2 * s.X, Const.OneMinusEpsilon);
            wi = Mathf.CosineSampleHemisphere(in s);
            if (wo.Y < 0)
            {
                wi.Y *= -1;
            }
        }
        else
        {
            s.X = Math.Min(2 * (s.X - 0.5f), Const.OneMinusEpsilon);
            var wh = distribution.Sample(in wo, in s);
            wi = wo.Reflect(wh);
            if (!Mathf.SameHemisphere(wo, wi))
            {
                pdf = 0;
                return Color.Black;
            }
        }
        pdf = Pdf(wo, wi);
        return Evaluate(in wo, in wi);
    }

    private float Pdf(Vector3 wo, Vector3 wi)
    {
        if (!Mathf.SameHemisphere(wo, wi))
        {
            return 0;
        }
        var wh = (wo + wi).Normalize();
        var pdf = distribution.Pdf(wo, wh);
        return 0.5f * (Mathf.AbsCosTheta(wi) * Const.InvPi + pdf / (4 * Vector3.Dot(wo, wh)));
    }

    private Color SchlickFresnel(float cosTheta)
    {
        var pow5 = Mathf.Pow5(1 - cosTheta);
        return specular + pow5 * (Color.White - specular);
    }
}
