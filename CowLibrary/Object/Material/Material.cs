namespace CowLibrary
{
    using System.Numerics;

    public interface IMaterial
    {
        public Color Color { get; }

        public Color GetColor(in Vector3 wo, in Vector3 wi);

        public float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, in Vector2 sample, out float pdf);
    }
}
