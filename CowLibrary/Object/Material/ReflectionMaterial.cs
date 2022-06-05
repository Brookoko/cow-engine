namespace CowLibrary
{
    using System.Numerics;

    public readonly struct ReflectionMaterial : IMaterial
    {
        public Color Color { get; }

        public int Id { get; }

        private readonly SpecularReflectionBrdf brdf;

        public ReflectionMaterial(Color color, float r, float eta, int id) :
            this(color, new SpecularReflectionBrdf(r, new DielectricFresnel(1, eta)), id)
        {
        }

        private ReflectionMaterial(Color color, SpecularReflectionBrdf brdf, int id)
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
            return new ReflectionMaterial(Color, brdf, id);
        }
    }
}
