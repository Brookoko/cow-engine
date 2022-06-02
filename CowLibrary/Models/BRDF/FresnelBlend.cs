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

    public Color Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal)
    {
        var toLocal = Mathf.GetLocal(in normal, in wi);
        var wiL = toLocal.MultiplyVector(wi);
        var woL = toLocal.MultiplyVector(wo);
        return EvaluateInternal(in woL, in wiL);
    }

    public Color Sample(in Vector3 normal, in Vector3 woW, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var s = sample;
        var (toLocal, toWorld) = Mathf.GetMatrices(in normal, in woW);
        var wo = -toLocal.MultiplyVector(woW);
        if (s.X < 0.5f)
        {
            s.X = Math.Min(2 * s.X, Const.OneMinusEpsilon);
            wi = Mathf.CosineSampleHemisphere(in sample);
            if (wo.Y < 0)
            {
                wi.Y *= -1;
            }
        }
        else
        {
            s.X = Math.Min(2 * (s.X - 0.5f), Const.OneMinusEpsilon);
            var wh = distribution.Sample(in wo, in sample);
            wi = wo.Reflect(wh);
            if (!Mathf.SameHemisphere(wo, wi))
            {
                wi.Y *= -1;
            }
        }
        pdf = Pdf(wo, wi);
        var f = EvaluateInternal(in wo, in wi);
        wi = toWorld.MultiplyVector(wi);
        return f;
    }

    private float Pdf(Vector3 wo, Vector3 wi)
    {
        if (!Mathf.SameHemisphere(wo, wi))
        {
            return 0;
        }
        var wh = (wo + wi).Normalize();
        var pdf = distribution.Pdf(wo, wh);
        var v = 0.5f * (Mathf.AbsCosTheta(wi) * Const.InvPi + pdf / (4 * Vector3.Dot(wo, wh)));
        return Mathf.Clamp(v, 0, 1);
    }

    private Color EvaluateInternal(in Vector3 wo, in Vector3 wi)
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

    private Color SchlickFresnel(float cosTheta)
    {
        var pow5 = Mathf.Pow5(1 - cosTheta);
        return specular + pow5 * (Color.White - specular);
    }
}
