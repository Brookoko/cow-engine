namespace CowLibrary
{
    using System.Numerics;

    public readonly struct DiffuseMaterial : IMaterial
    {
        public Color Color { get; }

        private readonly IBrdf brdf;

        public DiffuseMaterial(Color color, float r)
        {
            Color = color;
            brdf = new LambertianBrdf(r);
        }

        public Color GetColor(in Vector3 wo, in Vector3 wi)
        {
            return brdf.Evaluate(in wo, in wi) * Color;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, in Vector2 sample, out float pdf)
        {
            return brdf.Sample(in normal, in wo, out wi, in sample, out pdf);
        }
    }
}
