namespace CowLibrary;

using System.Numerics;
using Models.Microfacet;

public readonly struct MicrofacetReflection : IBrdf
{
    private readonly float r;
    private readonly DielectricFresnel fresnel;
    private readonly TrowbridgeReitzDistribution distribution;

    public MicrofacetReflection(float r, DielectricFresnel fresnel, TrowbridgeReitzDistribution distribution)
    {
        this.r = r;
        this.fresnel = fresnel;
        this.distribution = distribution;
    }

    public float Evaluate(in Vector3 wo, in Vector3 wi)
    {
        var cosThetaO = Mathf.AbsCosTheta(wo);
        var cosThetaI = Mathf.AbsCosTheta(wi);
        var wh = wi + wo;
        if (cosThetaO == 0 || cosThetaI == 0)
        {
            return 0;
        }
        if (wh.X == 0 && wh.Y == 0 && wh.Z == 0)
        {
            return 0;
        }
        wh = Vector3.Normalize(wh);
        var f = fresnel.Evaluate(Vector3.Dot(wi, Mathf.FaceForward(wh)));
        return r * distribution.D(in wh) * distribution.G(in wo, in wi) * f / (4 * cosThetaI * cosThetaO);
    }

    public float Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        wi = Vector3.Zero;
        pdf = 0;
        if (wo.Y == 0)
        {
            return 0f;
        }
        var wh = distribution.Sample(in wo, in sample);
        if (Vector3.Dot(wo, wh) < 0)
        {
            return 0f;
        }
        wi = wo.Reflect(wh);
        if (!Mathf.SameHemisphere(in wo, in wi))
        {
            return 0f;
        }
        pdf = distribution.Pdf(in wo, in wh) / (4 * Vector3.Dot(wo, wh));
        return Evaluate(in wo, in wi);
    }
}
