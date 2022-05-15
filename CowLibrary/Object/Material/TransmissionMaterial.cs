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

        public override Color GetColor(in Vector3 wo, in Vector3 wi)
        {
            return brdf.Evaluate(in wo, in wi) * Color;
        }

        public override float Sample(in Vector3 normal, in Vector3 wo, out Vector3 wi, out float pdf)
        {
            return brdf.Sample(in normal, in wo, out wi, sampler.CreateSample(), out pdf);
        }
    }
}
