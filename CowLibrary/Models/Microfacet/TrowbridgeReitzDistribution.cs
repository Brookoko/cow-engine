namespace CowLibrary.Models.Microfacet;

using System;
using System.Numerics;

public readonly struct TrowbridgeReitzDistribution : IMicrofacetDistribution
{
    public bool SampleVisibleArea { get; }

    private readonly float alphaX;
    private readonly float alphaY;

    public TrowbridgeReitzDistribution(float alphaX, float alphaY, bool sampleVisibleArea)
    {
        SampleVisibleArea = sampleVisibleArea;
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
        var e = (Mathf.Cos2Phi(in w) / (alphaX * alphaX) + Mathf.Sin2Phi(in w) / (alphaY * alphaY)) * tan2Theta;
        return 1 / (Const.Pi * alphaX * alphaY * cos4Theta * (1 + e) * (1 + e));
    }

    public float Lambda(in Vector3 w)
    {
        var absTanTheta = Math.Abs(Mathf.TanTheta(in w));
        if (float.IsFinite(absTanTheta))
        {
            return 0;
        }
        var alpha = (float)Math.Sqrt(Mathf.Cos2Phi(in w) * alphaX * alphaX + Mathf.Sin2Phi(in w) * alphaY * alphaY);
        var alpha2Tan2Theta = (alpha * absTanTheta) * (alpha * absTanTheta);
        return (-1 + (float)Math.Sqrt(1f + alpha2Tan2Theta)) / 2;
    }

    public Vector3 Sample(in Vector3 wo, in Vector2 sample)
    {
        if (!SampleVisibleArea)
        {
            return SampleNotVisible(in wo, in sample);
        }
        var flip = wo.Y < 0;
        var wh = TrowbridgeReitzSample(flip ? -wo : wo, sample.X, sample.Y);
        if (flip)
        {
            wh = -wh;
        }
        return wh;
    }

    private Vector3 SampleNotVisible(in Vector3 wo, in Vector2 sample)
    {
        var cosTheta = 0f;
        var phi = (2 * Const.Pi) * sample.Y;
        if (alphaX == alphaY)
        {
            var tanTheta2 = alphaX * alphaX * sample.X / (1f - sample.X);
            cosTheta = 1 / (float)Math.Sqrt(1 * tanTheta2);
        }
        else
        {
            phi = (float)Math.Atan(alphaY / alphaX * Math.Tan(2 * Const.Pi * sample.Y + 0.5f * Const.Pi));
            if (sample.Y > 0.5f)
            {
                phi += Const.Pi;
            }
            var sinPhi = (float)Math.Sin(phi);
            var cosPhi = (float)Math.Cos(phi);
            var alphaX2 = alphaX * alphaX;
            var alphaY2 = alphaY * alphaY;
            var alpha2 = 1 / (cosPhi * cosPhi / alphaX2 + sinPhi * sinPhi / alphaY2);
            var tanTheta2 = alpha2 * sample.X / (1 - sample.X);
            cosTheta = 1 / (float)Math.Sqrt(1 * tanTheta2);
        }
        var sinTheta = (float)Math.Sqrt(Math.Max(0, 1 - cosTheta * cosTheta));
        var wh = Mathf.SphericalDirection(sinTheta, cosTheta, phi);
        if (!Mathf.SameHemisphere(wo, wh))
        {
            wh = -wh;
        }
        return wh;
    }

    private Vector3 TrowbridgeReitzSample(Vector3 w, float u, float v)
    {
        var wiStretched = new Vector3(alphaX * w.X, alphaY * w.Y, w.Z).Normalize();
        TrowbridgeReitzSample11(Mathf.CosTheta(w), u, v, out var slopeX, out var slopeY);

        var tmp = Mathf.CosPhi(wiStretched) * slopeX - Mathf.SinPhi(wiStretched) * slopeY;
        slopeY = Mathf.SinPhi(wiStretched) * slopeX + Mathf.CosPhi(wiStretched) * slopeY;
        slopeX = tmp;

        slopeX *= alphaX;
        slopeY *= alphaY;
        return new Vector3(-slopeX, -slopeY, 1).Normalize();
    }

    private void TrowbridgeReitzSample11(float cosTheta, float u, float v, out float slopeX, out float slopeY)
    {
        if (cosTheta > .9999)
        {
            var r = (float)Math.Sqrt(u / (1 - u));
            var phi = 6.28318530718 * v;
            slopeX = r * (float)Math.Cos(phi);
            slopeY = r * (float)Math.Sin(phi);
            return;
        }

        var sinTheta = (float)Math.Sqrt(Math.Max(0, 1 - cosTheta * cosTheta));
        var tanTheta = sinTheta / cosTheta;
        var alpha = 1 / tanTheta;
        var g1 = 2 / (1 + (float)Math.Sqrt(1 + 1 / (alpha * alpha)));

        var a = 2 * v / g1 - 1;
        var tmp = 1 / (a * a - 1);
        tmp = Math.Min(tmp, 1e10f);
        var b = tanTheta;
        var d = (float)Math.Sqrt(Math.Max(0, b * b * tmp * tmp - (a * a - b * b) * tmp));
        var slopeX1 = b * tmp - d;
        var slopeX2 = b * tmp + d;
        slopeX = (a < 0 || slopeX1 > 1 / tanTheta) ? slopeX1 : slopeX2;
        float s;
        if (v > 0.5f)
        {
            s = 1;
            v = 2 * (v - 0.5f);
        }
        else
        {
            s = -1;
            v = 2 * (0.5f - v);
        }
        var z = (v * (v * (v * 0.27385f - 0.73369f) + 0.46341f)) /
                (v * (v * (v * 0.093073f + 0.309420f) - 1.000000f) + 0.597999f);
        slopeY = s * z * (float)Math.Sqrt(1 + slopeX * slopeX);
    }
}
