namespace CowLibrary
{
    using System;
    using System.Numerics;

    public readonly struct SpecularReflectionBrdf : IBrdf
    {
        private readonly float r;
        private readonly IFresnel fresnel;

        public SpecularReflectionBrdf(float r, IFresnel fresnel)
        {
            this.r = r;
            this.fresnel = fresnel;
        }

        public float Evaluate(in Vector3 wo, in Vector3 wi)
        {
            return 0;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, in Vector2 sample, out float pdf)
        {
            pdf = 1;
            wi = wo.Reflect(normal);
            var cos = Vector3.Dot(wi, normal);
            return fresnel.Evaluate(cos) * r / Math.Abs(cos);
        }
    }
}
