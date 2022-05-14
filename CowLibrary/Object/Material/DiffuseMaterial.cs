namespace CowLibrary
{
    using System.Numerics;
    using Mathematics.Sampler;

    public class DiffuseMaterial : Material
    {
        private readonly ISampler sampler;
        private readonly IBrdf brdf;

        public DiffuseMaterial(Color color, float r, ISampler sampler) : base(color)
        {
            this.sampler = sampler;
            brdf = new LambertianBrdf(r);
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
