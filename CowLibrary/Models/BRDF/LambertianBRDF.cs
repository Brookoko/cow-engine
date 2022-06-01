namespace CowLibrary
{
    using System;
    using System.Numerics;

    public readonly struct LambertianBrdf : IBrdf
    {
        private readonly float r;

        public LambertianBrdf(float r)
        {
            this.r = Math.Clamp(r, 0, 1);
        }

        public float Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal)
        {
            return r * Const.InvPi;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            wi = Mathf.CosineSampleHemisphere(in sample);
            var (toLocal, toWorld) = Mathf.GetMatrices(in normal, in wo);
            var woL = toLocal.MultiplyVector(wo);
            pdf = Mathf.Pdf(in woL, in wi);
            var f = Evaluate(in woL, in wi, in normal);
            wi = toWorld.MultiplyVector(wi);
            return f;
        }
    }
}
