namespace CowLibrary
{
    using System.Numerics;
    using Mathematics.Sampler;

    public class FresnelMaterial : Material
    {
        private readonly ISampler sampler;
        private readonly IBrdf brdf;

        public FresnelMaterial(float r, float t, float eta, ISampler sampler) : base(Color.White)
        {
            this.sampler = sampler;
            brdf = new FresnelSpecularBrdf(r, t, 1, eta, TransportMode.Importance);
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
