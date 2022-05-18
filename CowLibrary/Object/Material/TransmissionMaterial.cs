namespace CowLibrary
{
    using System.Numerics;

    public readonly struct TransmissionMaterial : IMaterial
    {
        public Color Color { get; }

        public int Id { get; }

        private readonly SpecularTransmissionBrdf brdf;

        public TransmissionMaterial(float t, float eta, int id) :
            this(new SpecularTransmissionBrdf(t, 1f, eta, TransportMode.Importance), id)
        {
        }

        private TransmissionMaterial(SpecularTransmissionBrdf brdf, int id)
        {
            Color = Color.White;
            this.brdf = brdf;
            Id = id;
        }

        public Color GetColor(in Vector3 wo, in Vector3 wi)
        {
            return brdf.Evaluate(in wo, in wi) * Color;
        }

        public float Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
        {
            return brdf.Sample(in normal, in wo, in sample, out wi, out pdf);
        }

        public IMaterial Copy(int id)
        {
            return new TransmissionMaterial(brdf, id);
        }
    }
}
