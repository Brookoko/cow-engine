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

        public float Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            wi = Mathf.CosineSampleHemisphere(in sample);
            if (wo.Y < 0)
            {
                wi.Y *= -1;
            }
            pdf = Pdf(in wo, in wi);
            return Evaluate(in wo, in wi);
        }

        public float Pdf(in Vector3 wo, in Vector3 wi)
        {
            return Mathf.SameHemisphere(in wo, in wi) ? Mathf.AbsCosTheta(in wi) * Const.InvPi : 0;
        }
    }
}
