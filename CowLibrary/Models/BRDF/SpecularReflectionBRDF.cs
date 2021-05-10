namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class SpecularReflectionBRDF : BRDF
    {
        private readonly float r;
        private readonly Fresnel fresnel;
        
        public SpecularReflectionBRDF(float r, Fresnel fresnel)
        {
            this.r = r;
            this.fresnel = fresnel;
        }
        
        public override float Evaluate(Vector3 wo, Vector3 wi)
        {
            return 0;
        }
        
        public override float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf)
        {
            pdf = 1;
            var wo = surfel.ray;
            wi = wo.Reflect(surfel.normal);
            var cos = Vector3.Dot(wi, surfel.normal);
            return fresnel.Evaluate(cos) * r / Math.Abs(cos);
        }
    }
}