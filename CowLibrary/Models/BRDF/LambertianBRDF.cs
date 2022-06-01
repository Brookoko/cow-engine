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

        public float Evaluate(in Vector3 wo, in Vector3 wi)
        {
            return r * Const.InvPi;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            wi = Mathf.CosineSampleHemisphere(normal, sample);
            if (Vector3.Dot(wo, normal) < 0)
            {
                wi.Z *= -1;
            }
            pdf = Pdf(in wo, in wi, in normal);
            return Evaluate(wo, wi);
        }

        private float Pdf(in Vector3 wo, in Vector3 wi, in Vector3 normal)
        {
            return Mathf.SameHemisphere(in wo, in wi, in normal) ? Mathf.AbsCosTheta(in wi, in normal) : 0;
        }
    }
}
