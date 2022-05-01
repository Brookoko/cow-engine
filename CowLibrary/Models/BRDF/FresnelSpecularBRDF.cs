namespace CowLibrary
{
    using System.Numerics;

    public class FresnelSpecularBrdf : IBrdf
    {
        private readonly DielectricFresnel fresnel;
        private readonly IBrdf reflection;
        private readonly IBrdf transmission;

        public FresnelSpecularBrdf(float r, float t, float etaA, float etaB, TransportMode mode)
        {
            fresnel = new DielectricFresnel(etaA, etaB);
            reflection = new SpecularReflectionBrdf(r, fresnel);
            transmission = new SpecularTransmissionBrdf(t, etaA, etaB, mode);
        }

        public float Evaluate(Vector3 wo, Vector3 wi)
        {
            return 0;
        }

        public float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf)
        {
            float res;
            var f = fresnel.Evaluate(Vector3.Dot(surfel.normal, surfel.ray));
            if (sample.X < f)
            {
                res = reflection.Sample(surfel, out wi, sample, out pdf);
                pdf = f;
            }
            else
            {
                res = transmission.Sample(surfel, out wi, sample, out pdf);
                pdf = 1 - f;
            }
            return res;
        }
    }
}
