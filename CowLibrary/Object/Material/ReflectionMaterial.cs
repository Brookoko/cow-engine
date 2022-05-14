namespace CowLibrary
{
    using System.Numerics;
    using Mathematics.Sampler;

    public class ReflectionMaterial : Material
    {
        private readonly ISampler sampler;
        private readonly IBrdf brdf;

        public ReflectionMaterial(float r, float eta, ISampler sampler) : base(Color.White)
        {
            this.sampler = sampler;
            var fresnel = new DielectricFresnel(1, eta);
            brdf = new SpecularReflectionBrdf(r, fresnel);
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
