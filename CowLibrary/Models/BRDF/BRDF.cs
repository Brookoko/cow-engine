namespace CowLibrary
{
    using System.Numerics;

    public abstract class BRDF
    {
        public abstract float Evaluate(Vector3 wo, Vector3 wi);
        
        public abstract float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf);
    }
}