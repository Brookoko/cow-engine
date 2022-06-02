namespace CowLibrary;

using System.Numerics;
using Models.Microfacet;

public class MicrofacetReflection : IBrdf
{
    private readonly float r;
    private readonly DielectricFresnel fresnel;
    private readonly BeckmannDistribution distribution;

    public MicrofacetReflection(float r, DielectricFresnel fresnel, BeckmannDistribution distribution)
    {
        this.r = r;
        this.fresnel = fresnel;
        this.distribution = distribution;
    }

    public float Evaluate(in Vector3 woW, in Vector3 wiW, in Vector3 normal)
    {
        var toLocal = Mathf.GetLocal(in normal, in wiW);
        var wo = toLocal.MultiplyVector(woW);
        var wi = toLocal.MultiplyVector(wiW);
        return EvaluateInternal(in wo, in wi);
    }

    public float Sample(in Vector3 normal, in Vector3 woW, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var (toLocal, toWorld) = Mathf.GetMatrices(in normal, in woW);
        var wo = toLocal.MultiplyVector(woW);
        var wh = distribution.Sample(in wo, sample);
        wi = Vector3.Zero;
        pdf = 0;
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
        var f = EvaluateInternal(in wo, in wi);
        wi = toWorld.MultiplyVector(wi);
        return f;
    }

    public float EvaluateInternal(in Vector3 wo, in Vector3 wi)
    {
        var cosThetaO = Mathf.CosTheta(wo);
        var cosThetaI = Mathf.CosTheta(wi);
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
        var f = fresnel.Evaluate(Vector3.Dot(wi, wh));
        return r * distribution.D(in wh) * distribution.G(in wo, in wi) * f / (4 * cosThetaI * cosThetaO);
    }
}
