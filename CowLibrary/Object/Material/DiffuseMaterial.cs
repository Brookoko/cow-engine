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
