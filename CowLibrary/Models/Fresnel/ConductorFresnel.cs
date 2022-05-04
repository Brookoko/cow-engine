namespace CowLibrary
{
    using System;

    public class ConductorFresnel : Fresnel
    {
        private readonly float k;

        public ConductorFresnel(float etaI, float etaT, float k) : base(etaI, etaT)
        {
            this.k = k;
        }

        public override float Evaluate(float cosThetaI)
        {
            cosThetaI = Math.Clamp(cosThetaI, -1, 1);
            var eta = etaT / etaI;
            var etaK = k / etaI;

            var cosThetaI2 = cosThetaI * cosThetaI;
            var sinThetaI2 = 1 - cosThetaI2;
            var eta2 = eta * eta;
            var etaK2 = etaK * etaK;

            var t0 = eta2 - etaK2 - sinThetaI2;
            var a2PlusB2 = Math.Sqrt(t0 * t0 + 4 * eta2 * etaK2);
            var t1 = a2PlusB2 + cosThetaI2;
            var a = Math.Sqrt(0.5f * (a2PlusB2 + t0));
            var t2 = 2 * cosThetaI * a;
            var rs = (t1 - t2) / (t1 + t2);

            var t3 = cosThetaI2 * a2PlusB2 + sinThetaI2 * sinThetaI2;
            var t4 = t2 * sinThetaI2;
            var rp = rs * (t3 - t4) / (t3 + t4);

            return (float)(rp + rs) * 0.5f;
        }
    }
}
