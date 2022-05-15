namespace CowLibrary
{
    using System.Numerics;

    public abstract class Material
    {
        public Color Color { get; }

        protected Material(Color color)
        {
            Color = color;
        }

        public abstract Color GetColor(in Vector3 wo, in Vector3 wi);

        public abstract float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, out float pdf);
    }
}
