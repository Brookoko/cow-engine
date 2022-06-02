namespace CowLibrary;

using System;
using System.Numerics;

public readonly struct OrenNayar : IBrdf
{
    private readonly float r;
    private const float Epsilon = 1e-4f;

    private readonly float a;
    private readonly float b;

    public OrenNayar(float r, float sigma)
    {
        this.r = r;
        sigma = Mathf.Clamp(sigma, 0, 90);
        sigma *= Const.Deg2Rad;
        var sigma2 = sigma * sigma;
        a = 1 - sigma2 / (2 * (sigma2 + 0.33f));
        b = 0.45f * sigma2 / (sigma2 + 0.09f);
    }

    public float Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal)
    {
        var toLocal = Mathf.GetLocal(in normal, in wi);
        var wiL = toLocal.MultiplyVector(wi);
        var woL = toLocal.MultiplyVector(wo);
        return EvaluateInternal(in woL, in wiL);
    }

    public float Sample(in Vector3 normal, in Vector3 woW, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        wi = Mathf.CosineSampleHemisphere(in sample);
        var (toLocal, toWorld) = Mathf.GetMatrices(in normal, in woW);
        var wo = -toLocal.MultiplyVector(woW);
        if (wo.Y < 0)
        {
            wi.Y *= -1;
        }
        pdf = Mathf.Pdf(in wo, in wi);
        var f = Evaluate(in wo, in wi, in normal);
        wi = toWorld.MultiplyVector(wi);
        return f;
    }

    private float EvaluateInternal(in Vector3 wo, in Vector3 wi)
    {
        var sinThetaI = Mathf.SinTheta(in wi);
        var sinThetaO = Mathf.SinTheta(in wo);
        var maxCos = 0f;
        if (sinThetaI > Epsilon && sinThetaO > Epsilon)
        {
            var sinPhiI = Mathf.SinPhi(in wi, sinThetaI);
            var cosPhiI = Mathf.CosPhi(in wi, sinThetaI);
            var sinPhiO = Mathf.SinPhi(in wo, sinThetaO);
            var cosPhiO = Mathf.SinPhi(in wo, sinThetaO);
            var dCos = cosPhiI * cosPhiO + sinPhiI * sinPhiO;
            maxCos = Math.Max(0, dCos);
        }
        float sinAlpha, tanBeta;
        if (Mathf.AbsCosTheta(in wi) > Mathf.AbsCosTheta(in wo))
        {
            sinAlpha = sinThetaO;
            tanBeta = sinThetaI / Mathf.AbsCosTheta(in wi);
        }
        else
        {
            sinAlpha = sinThetaI;
            tanBeta = sinThetaO / Mathf.AbsCosTheta(in wo);
        }

        var f = r * Const.InvPi * (a + b * maxCos * sinAlpha * tanBeta);
        return Mathf.Clamp(f, 0, 1);
    }
}
