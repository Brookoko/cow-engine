namespace CowLibrary
{
    using System.Numerics;

    public readonly struct FresnelMaterial : IMaterial
    {
        public Color Color { get; }

        public int Id { get; }

        private readonly FresnelSpecularBrdf brdf;

        public FresnelMaterial(float r, float t, float eta, int id) :
            this(new FresnelSpecularBrdf(r, t, 1, eta, TransportMode.Importance), id)
        {
        }

        private FresnelMaterial(FresnelSpecularBrdf brdf, int id)
        {
            Color = Color.White;
            this.brdf = brdf;
            Id = id;
        }

        public Color GetColor(in Vector3 wo, in Vector3 wi)
        {
            return brdf.Evaluate(in wo, in wi) * Color;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, in Vector2 sample, out float pdf)
        {
            return brdf.Sample(in normal, in wo, out wi, in sample, out pdf);
        }

        public IMaterial Copy(int id)
        {
            return new FresnelMaterial(brdf, id);
        }
    }
}
