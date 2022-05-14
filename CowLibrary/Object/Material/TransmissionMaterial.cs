namespace CowLibrary
{
    using System.Numerics;
    using Mathematics.Sampler;

    public class TransmissionMaterial : Material
    {
        private readonly ISampler sampler;
        private readonly IBrdf brdf;

        public TransmissionMaterial(float t, float eta, ISampler sampler) : base(Color.White)
        {
            this.sampler = sampler;
            brdf = new SpecularTransmissionBrdf(t, 1f, eta, TransportMode.Importance);
        }

        public override Color GetColor(Vector3 wo, Vector3 wi)
        {
            return brdf.Evaluate(wo, wi) * Color;
        }

        public override float Sample(in Surfel surfel, out Vector3 wi, out float pdf)
        {
            return brdf.Sample(surfel, out wi, sampler.CreateSample(), out pdf);
        }
    }
}
