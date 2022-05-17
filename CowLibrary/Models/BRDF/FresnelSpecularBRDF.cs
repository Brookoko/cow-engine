namespace CowLibrary
{
    using System.Numerics;

    public readonly struct FresnelSpecularBrdf : IBrdf
    {
        private readonly DielectricFresnel fresnel;
        private readonly SpecularReflectionBrdf reflection;
        private readonly SpecularTransmissionBrdf transmission;

        public FresnelSpecularBrdf(float r, float t, float etaA, float etaB, TransportMode mode)
        {
            fresnel = new DielectricFresnel(etaA, etaB);
            reflection = new SpecularReflectionBrdf(r, fresnel);
            transmission = new SpecularTransmissionBrdf(t, etaA, etaB, mode);
        }

        public float Evaluate(in Vector3 wo, in Vector3 wi)
        {
            return 0;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, in Vector2 sample, out float pdf)
        {
            float res;
            var f = fresnel.Evaluate(Vector3.Dot(normal, wo));
            if (sample.X < f)
            {
                res = reflection.Sample(in normal, in wo, out wi, in sample, out pdf);
                pdf = f;
            }
            else
            {
                res = transmission.Sample(in normal, in wo, out wi, in sample, out pdf);
                pdf = 1 - f;
            }
            return res;
        }
    }
}
