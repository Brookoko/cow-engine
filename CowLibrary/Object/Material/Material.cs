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

        public abstract Color GetColor(Vector3 wo, Vector3 wi);

        public abstract float Sample(in Surfel surfel, out Vector3 wi, out float pdf);
    }
}
