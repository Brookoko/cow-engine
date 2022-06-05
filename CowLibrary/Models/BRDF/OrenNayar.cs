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
        sigma *= Const.Pi * 0.5f;
        var sigma2 = sigma * sigma;
        a = 1 - sigma2 / (2 * (sigma2 + 0.33f));
        b = 0.45f * sigma2 / (sigma2 + 0.09f);
    }

    public float Evaluate(in Vector3 wo, in Vector3 wi)
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

    public float Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        wi = Mathf.CosineSampleHemisphere(in sample);
        if (wo.Z < 0)
        {
            wi.Z *= -1;
        }
        pdf = Pdf(in wo, in wi);
        return Evaluate(in wo, in wi);
    }
    
    public float Pdf(in Vector3 wo, in Vector3 wi)
    {
        return Mathf.SameHemisphere(in wo, in wi) ? Mathf.AbsCosTheta(in wi) * Const.InvPi : 0;
    }
}
