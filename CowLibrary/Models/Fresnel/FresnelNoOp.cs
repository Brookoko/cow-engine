namespace CowLibrary
{
    public readonly struct FresnelNoOp : IFresnel
    {
        public float Evaluate(float cosThetaI)
        {
            return 1;
        }
    }
}
