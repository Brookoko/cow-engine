namespace CowLibrary
{
    public abstract class Fresnel
    {
        protected readonly float etaI;
        protected readonly float etaT;

        protected Fresnel(float etaI, float etaT)
        {
            this.etaI = etaI;
            this.etaT = etaT;
        }

        public abstract float Evaluate(float cosThetaI);
    }
}
