namespace CowLibrary
{
    using System;
    using System.Numerics;

    public readonly struct SpecularReflectionBrdf : IBrdf
    {
        private readonly float r;
        private readonly DielectricFresnel fresnel;

        public SpecularReflectionBrdf(float r, DielectricFresnel fresnel)
        {
            this.r = r;
            this.fresnel = fresnel;
        }

        public float Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal)
        {
            return 0;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            pdf = 1;
            wi = wo.Reflect(normal);
            var cos = Vector3.Dot(wi, normal);
            return fresnel.Evaluate(cos) * r / Math.Abs(cos);
        }
    }
}
