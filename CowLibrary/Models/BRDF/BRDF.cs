namespace CowLibrary
{
    using System.Numerics;

    public interface IBrdf
    {
        public float Evaluate(Vector3 wo, Vector3 wi);

        public float Sample(Surfel surfel, out Vector3 wi, Vector2 sample, out float pdf);
    }
}
