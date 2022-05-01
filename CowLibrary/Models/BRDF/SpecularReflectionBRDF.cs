namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class SpecularReflectionBrdf : IBrdf
    {
        private readonly float r;
        private readonly Fresnel fresnel;

        public SpecularReflectionBrdf(float r, Fresnel fresnel)
        {
            this.r = r;
            this.fresnel = fresnel;
        }

        public float Evaluate(Vector3 wo, Vector3 wi)
        {
            return 0;
        }

        public float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf)
        {
            pdf = 1;
            var wo = surfel.ray;
            wi = wo.Reflect(surfel.normal);
            var cos = Vector3.Dot(wi, surfel.normal);
            return fresnel.Evaluate(cos) * r / Math.Abs(cos);
        }
    }
}
