namespace CowLibrary
{
    public class FresnelNoOp : Fresnel
    {
        public FresnelNoOp() : base(1, 1)
        {
        }
        
        public FresnelNoOp(float etaI, float etaT) : base(etaI, etaT)
        {
        }
        
        public override float Evaluate(float cosThetaI)
        {
            return 1;
        }
    }
}