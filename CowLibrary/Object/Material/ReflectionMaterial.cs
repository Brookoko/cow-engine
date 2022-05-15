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
