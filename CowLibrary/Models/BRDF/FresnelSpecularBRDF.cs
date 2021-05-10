namespace CowLibrary
{
    using System.Numerics;

    public class FresnelSpecularBRDF : BRDF
    {
        private readonly DielectricFresnel fresnel;
        private readonly BRDF reflection;
        private readonly BRDF transmission;
        
        public FresnelSpecularBRDF(float r, float t, float etaA, float etaB, TransportMode mode)
        {
            fresnel = new DielectricFresnel(etaA, etaB);
            reflection = new SpecularReflectionBRDF(r, fresnel);
            transmission = new SpecularTransmissionBRDF(t, etaA, etaB, mode);
        }
        
        public override float Evaluate(Vector3 wo, Vector3 wi)
        {
            return 0;
        }
        
        public override float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf)
        {
            var f = fresnel.Evaluate(Vector3.Dot(surfel.normal, surfel.ray));
            if (sample.X < f)
            {
                return reflection.Sample(surfel, out wi, sample, out pdf);
            }
            return transmission.Sample(surfel, out wi, sample, out pdf);
        }
    }
}