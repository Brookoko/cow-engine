namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class LambertianBrdf : IBrdf
    {
        private readonly float r;

        public LambertianBrdf(float r)
        {
            this.r = Mathf.Clamp(r, 0, 1);
        }

        public float Evaluate(Vector3 wo, Vector3 wi)
        {
            return r * Const.InvPi;
        }

        public float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf)
        {
            var wo = surfel.ray;
            var up = surfel.normal;
            wi = Mathf.CosineSampleHemisphere(up, sample);
            if (Vector3.Dot(wo, up) < 0)
            {
                wi.Z *= -1;
            }
            pdf = sample.X;
            return Evaluate(wo, wi);
        }
    }
}
