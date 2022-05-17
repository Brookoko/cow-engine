namespace CowLibrary
{
    using System.Numerics;

    public readonly struct ReflectionMaterial : IMaterial
    {
        public Color Color { get; }

        private readonly SpecularReflectionBrdf brdf;

        public ReflectionMaterial(float r, float eta)
        {
            Color = Color.White;
            var fresnel = new DielectricFresnel(1, eta);
            brdf = new SpecularReflectionBrdf(r, fresnel);
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
