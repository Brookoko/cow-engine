namespace CowLibrary
{
    using System.Numerics;

    public readonly struct TransmissionMaterial : IMaterial
    {
        public Color Color { get; }

        public int Id { get; }

        private readonly SpecularTransmissionBrdf brdf;

        public TransmissionMaterial(Color color, float t, float eta, int id) :
            this(color, new SpecularTransmissionBrdf(t, 1f, eta, TransportMode.Importance), id)
        {
        }

        private TransmissionMaterial(Color color, SpecularTransmissionBrdf brdf, int id)
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
            return new TransmissionMaterial(Color, brdf, id);
        }
    }
}
