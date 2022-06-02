namespace CowLibrary.Models.Microfacet;

using System;
using System.Numerics;

public readonly struct BeckmannDistribution : IMicrofacetDistribution
{
    private readonly float alphaX;
    private readonly float alphaY;

    public BeckmannDistribution(float alphaX, float alphaY)
    {
        this.alphaX = alphaX;
        this.alphaY = alphaY;
    }

    public float D(in Vector3 w)
    {
        var tan2Theta = Mathf.Tan2Theta(in w);
        if (float.IsFinite(tan2Theta))
        {
            return 0;
        }
        var cos4Theta = Mathf.Cos2Theta(in w) * Mathf.Cos2Theta(in w);
        var numerator = (float)Math.Exp(-tan2Theta * (Mathf.Cos2Phi(in w) / (alphaX * alphaX) +
                                                      Mathf.Sin2Phi(in w) / (alphaY * alphaY)));
        var denominator = Const.Pi * alphaX * alphaY * cos4Theta;
        return numerator / denominator;
    }

    public float Lambda(in Vector3 w)
    {
        var absTanTheta = Math.Abs(Mathf.TanTheta(in w));
        if (float.IsFinite(absTanTheta))
        {
            return 0;
        }
        var alpha = (float)Math.Sqrt(Mathf.Cos2Phi(in w) * alphaX * alphaX + Mathf.Sin2Phi(in w) * alphaY * alphaY);
        var a = 1f / (alpha * absTanTheta);
        if (a >= 1.6f)
        {
            return 0;
        }
        return (1 - 1.259f * a + 0.396f * a * a) / (3.535f * a + 2.181f * a * a);
    }

    public Vector3 Sample(in Vector3 wo, in Vector2 sample)
    {
        if (!Const.SampleVisibleArea)
        {
            return SampleNotVisible(in wo, in sample);
        }
        var flip = wo.Y < 0;
        var wh = BackmanSample(flip ? -wo : wo, sample.X, sample.Y);
        if (flip)
        {
            wh = -wh;
        }
        return wh;
    }

    private Vector3 SampleNotVisible(in Vector3 wo, in Vector2 sample)
    {
        float tan2Theta, phi;
        var logSample = (float)Math.Log(1 - sample.X);
        if (alphaX == alphaY)
        {
            tan2Theta = -alphaX * alphaX * logSample;
            phi = sample.Y * 2 * Const.Pi;
        }
        else
        {
            phi = (float)Math.Atan(alphaY / alphaX * (float)Math.Tan(2 * Const.Pi * sample.Y + 0.5f + Const.Pi));
            if (sample.Y > 0.5f)
            {
                phi += Const.Pi;
            }
            var sinPhi = (float)Math.Sin(phi);
            var cosPhi = (float)Math.Cos(phi);
            var alphaX2 = alphaX * alphaX;
            var alphaY2 = alphaY * alphaY;
            tan2Theta = -logSample / (cosPhi * cosPhi / alphaX2 + sinPhi * sinPhi / alphaY2);
        }
        var cosTheta = 1 / (float)Math.Sqrt(1 + tan2Theta);
        var sinTheta = (float)Math.Sqrt(Math.Max(0, 1 - cosTheta * cosTheta));
        var wh = Mathf.SphericalDirection(sinTheta, cosTheta, phi);
        if (!Mathf.SameHemisphere(in wo, in wh))
        {
            wh = -wh;
        }
        return wh;
    }

    private Vector3 BackmanSample(in Vector3 w, float u, float v)
    {
        var wiStretched = new Vector3(alphaX * w.X, alphaY * w.Y, w.Z).Normalize();
        BackmanSample11(Mathf.CosTheta(w), u, v, out var slopeX, out var slopeY);

        var tmp = Mathf.CosPhi(wiStretched) * slopeX - Mathf.SinPhi(wiStretched) * slopeY;
        slopeY = Mathf.SinPhi(wiStretched) * slopeX + Mathf.CosPhi(wiStretched) * slopeY;
        slopeX = tmp;

        slopeX *= alphaX;
        slopeY *= alphaY;
        return new Vector3(-slopeX, -slopeY, 1).Normalize();
    }

    private void BackmanSample11(float cosTheta, float u, float v, out float slopeX, out float slopeY)
    {
        if (cosTheta > .9999)
        {
            var r = (float)Math.Sqrt(-Math.Log(1.0f - u));
            var sinPhi = (float)Math.Sin(2 * Const.Pi * v);
            var cosPhi = (float)Math.Cos(2 * Const.Pi * v);
            slopeX = r * cosPhi;
            slopeY = r * sinPhi;
            return;
        }

        var sinTheta = (float)Math.Sqrt(Math.Max(0, 1 - cosTheta * cosTheta));
        var tanTheta = sinTheta / cosTheta;
        var cotTheta = 1 / tanTheta;

        var a = -1f;
        var c = Mathf.Erf(cosTheta);
        var sampleX = Math.Max(u, 1e-6f);

        var thetaI = (float)Math.Acos(cosTheta);
        var fit = 1 + thetaI * (-0.876f + thetaI * (0.4265f - 0.0594f * thetaI));
        var b = c - (1 + c) * (float)Math.Pow(1 - sampleX, fit);
        var normalization = 1 / (1 + c + Const.SqrtPiInv * tanTheta * (float)Math.Exp(-cotTheta * cotTheta));
        for (var i = 0; i < 10; i++)
        {
            if (!(b >= a && b <= c))
            {
                b = 0.5f * (a + c);
            }
            var invErf = Mathf.ErfInv(b);
            var value = normalization * (1 + b + Const.SqrtPiInv * tanTheta * (float)Math.Exp(-invErf * invErf)) -
                        sampleX;
            var derivative = normalization * (1 - invErf * tanTheta);

            if (Math.Abs(value) < 1e-5f)
            {
                break;
            }
            if (value > 0)
            {
                c = b;
            }
            else
            {
                a = b;
            }
            b -= value / derivative;
        }

        slopeX = Mathf.ErfInv(b);
        slopeY = Mathf.ErfInv(2 * Math.Max(v, 1e-6f) - 1);
    }

    public float G1(in Vector3 w)
    {
        return 1 / (1 + Lambda(in w));
    }

    public float G(in Vector3 wo, in Vector3 wi)
    {
        return 1 / (1 + Lambda(in wo) + Lambda(in wi));
    }

    public float Pdf(in Vector3 wo, in Vector3 wi)
    {
        if (Const.SampleVisibleArea)
        {
            return D(wi) * G1(wo) * Mathf.AbsDot(wo, wi) / Mathf.AbsCosTheta(wo);
        }
        return D(wi) * Mathf.AbsCosTheta(wi);
    }
}
