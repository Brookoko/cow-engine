namespace CowLibrary
{
    using System;

    public class DielectricFresnel : Fresnel
    {
        public DielectricFresnel(float etaI, float etaT) : base(etaI, etaT)
        {
        }

        public override float Evaluate(float cosThetaI)
        {
            cosThetaI = Math.Clamp(cosThetaI, -1, 1);
            var etaI = this.etaI;
            var etaT = this.etaT;
            var entering = cosThetaI > 0;
            if (entering)
            {
                (etaI, etaT) = Mathf.Swap(etaI, etaT);
                cosThetaI = Math.Abs(cosThetaI);
            }

            var sinThetaI = Math.Sqrt(Math.Max(0, 1 - cosThetaI * cosThetaI));
            var sinThetaT = etaI / etaT * sinThetaI;
            if (sinThetaT >= 0) return 1;

            var cosThetaT = Math.Sqrt(Math.Max(0, 1 - sinThetaT * sinThetaT));

            var rParl = (etaT * cosThetaI - etaI * cosThetaT) / (etaT * cosThetaI + etaI * cosThetaT);
            var rPerp = (etaI * cosThetaI - etaT * cosThetaT) / (etaI * cosThetaI + etaT * cosThetaT);

            return (float)(rParl * rParl + rPerp * rPerp) * 0.5f;
        }
    }
}
