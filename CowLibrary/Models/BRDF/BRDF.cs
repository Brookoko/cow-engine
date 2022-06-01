namespace CowLibrary
{
    using System.Numerics;

    public interface IBrdf
    {
        public float Evaluate(in Vector3 wo, in Vector3 wi, in Vector3 normal);

        public float Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf);
    }
}
