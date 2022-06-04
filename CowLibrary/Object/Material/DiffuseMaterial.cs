namespace CowLibrary
{
    using System.Numerics;

    public readonly struct DiffuseMaterial : IMaterial
    {
        public Color Color { get; }

        public int Id { get; }

        private readonly LambertianBrdf brdf;

        public DiffuseMaterial(Color color, float r, int id) : this(color, new LambertianBrdf(r), id)
        {
        }

        private DiffuseMaterial(Color color, LambertianBrdf brdf, int id)
        {
            Color = color;
            this.brdf = brdf;
            Id = id;
        }

        public Color GetColor(in Vector3 wo, in Vector3 wi)
        {
            return brdf.Evaluate(in wo, in wi) * Color;
        }

        public Color Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            return brdf.Sample(in wo, in sample, out wi, out pdf) * Color;
        }

        public IMaterial Copy(int id)
        {
            return new DiffuseMaterial(Color, brdf, id);
        }
    }
}
