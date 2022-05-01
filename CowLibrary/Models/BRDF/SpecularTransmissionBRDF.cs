namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class SpecularTransmissionBrdf : IBrdf
    {
        private readonly float t;
        private readonly float etaA;
        private readonly float etaB;
        private readonly TransportMode mode;
        private readonly DielectricFresnel fresnel;
        
        public SpecularTransmissionBrdf(float t, float etaA, float etaB, TransportMode mode)
        {
            this.t = t;
            this.etaA = etaA;
            this.etaB = etaB;
            this.mode = mode;
            fresnel = new DielectricFresnel(etaA, etaB);
        }
        
        public float Evaluate(Vector3 wo, Vector3 wi)
        {
            return 0;
        }
        
        public float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf)
        {
            var wo = surfel.ray;
            var entering = Vector3.Dot(wo, surfel.normal) < 0;
            var etaI = entering ? etaA : etaB;
            var etaT = entering ? etaB : etaA;
            var eta = etaI / etaT;
            var n = Vector3Extensions.Faceforward(surfel.normal, wo);

            if (!wo.Refract(n, eta, out wi))
            {
                pdf = 0;
                return 0;
            }
            
            pdf = 1;
            var cos = Vector3.Dot(wi, surfel.normal);
            var ft = t * (1 - fresnel.Evaluate(cos));
            if (mode == TransportMode.Radiance)
            {
                ft *= (etaI * etaI) / (etaT * etaT);
            }
            return ft / Math.Abs(cos);
        }
    }

    public enum TransportMode
    {
        Radiance,
        Importance
    }
}