namespace CowLibrary
{
    using System;

    public readonly struct DielectricFresnel : IFresnel
    {
        private readonly float etaI;
        private readonly float etaT;
        
        public DielectricFresnel(float etaI, float etaT)
        {
            this.etaI = etaI;
            this.etaT = etaT;
        }

        public float Evaluate(float cosThetaI)
        {
            cosThetaI = Math.Min(Math.Max(cosThetaI, -1), 1);
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
