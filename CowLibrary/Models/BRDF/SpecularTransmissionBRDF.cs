namespace CowLibrary
{
    using System;
    using System.Numerics;

    public readonly struct SpecularTransmissionBrdf : IBrdf
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

        public float Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal)
        {
            return 0;
        }

        public float Sample(in Vector3 normal, in Vector3 woW, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            var (toLocal, toWorld) = Mathf.GetMatrices(in normal, in woW);
            var wo = -toLocal.MultiplyVector(woW);
            
            var entering = Mathf.CosTheta(wo) > 0;
            var etaI = entering ? etaA : etaB;
            var etaT = entering ? etaB : etaA;
            var eta = etaI / etaT;
            var n = Mathf.FaceForward(wo, Vector3.UnitY);

            if (!wo.Refract(n, eta, out wi))
            {
                pdf = 0;
                return 0;
            }

            pdf = 1;
            var ft = t * (1 - fresnel.Evaluate(Mathf.CosTheta(wi)));
            if (mode == TransportMode.Radiance)
            {
                ft *= (etaI * etaI) / (etaT * etaT);
            }
            var f = ft / Mathf.AbsCosTheta(wi);
            wi = toWorld.MultiplyVector(wi);
            return f;
        }
    }

    public enum TransportMode
    {
        Radiance,
        Importance
    }
}
