namespace CowLibrary
{
    using System.Numerics;

    public readonly struct TransmissionMaterial : IMaterial
    {
        public Color Color { get; }

        private readonly SpecularTransmissionBrdf brdf;

        public TransmissionMaterial(float t, float eta)
        {
            Color = Color.White;
            brdf = new SpecularTransmissionBrdf(t, 1f, eta, TransportMode.Importance);
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
