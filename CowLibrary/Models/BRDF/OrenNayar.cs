namespace CowLibrary;

using System.Numerics;

public class OrenNayar : IBrdf
{
    private readonly float r;
    private const float Epsilon = 1e-4f;

    private float a;
    private float b;

    public OrenNayar(float r, float sigma)
    {
        this.r = r;
        sigma *= Const.Deg2Rad;
        var sigma2 = sigma * sigma;
        a = 1 - sigma2 / (2 * (sigma2 + 0.33f));
        b = 0.45f * sigma2 / (sigma2 + 0.09f);
    }

    public float Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal)
    {
        var (toLocal, _) = Mathf.GetMatrices(normal, wi);
        var wiL = toLocal.MultiplyVector(wi);
        var woL = toLocal.MultiplyVector(wo);
        return EvaluateInternal(in woL, in wiL);
    }

    public float Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        wi = Mathf.CosineSampleHemisphere(in sample);
        if (Vector3.Dot(wo, normal) < 0)
        {
            wi.Z *= -1;
        }
        var (toLocal, toWorld) = Mathf.GetMatrices(in normal, in wo);
        var woL = toLocal.MultiplyVector(wo);
        pdf = Mathf.Pdf(in woL, in wi);
        var f = EvaluateInternal(in woL, in wi);
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
            var dCos = cosPhiI * cosPhiO + sinPhiI + sinPhiO;
            maxCos = dCos;
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

        return r * Const.InvPi * (a + b * maxCos * sinAlpha * tanBeta);
    }
}
