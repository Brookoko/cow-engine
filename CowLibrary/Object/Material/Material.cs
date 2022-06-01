namespace CowLibrary
{
    using System.Numerics;

    public interface IMaterial
    {
        public Color Color { get; }

        public int Id { get; }

        public Color GetColor(in Vector3 wo, in Vector3 wi, in Vector3 normal);

        public Color Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf);

        IMaterial Copy(int id);
    }
}
