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

        public float Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            float res;
            var f = fresnel.Evaluate(Mathf.CosTheta(in wo));
            if (sample.X < f)
            {
                res = reflection.Sample(in wo, in sample, out wi, out pdf);
                pdf = f;
            }
            else
            {
                res = transmission.Sample(in wo, in sample, out wi, out pdf);
                pdf = 1 - f;
            }
            return res;
        }
    }
}
