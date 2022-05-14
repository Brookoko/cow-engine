namespace CowLibrary
{
    using System.Numerics;

    public class ReflectionMaterial : Material
    {
        private IBrdf brdf;

        public ReflectionMaterial(float r, float eta) : base(Color.White)
        {
            var fresnel = new DielectricFresnel(1, eta);
            brdf = new SpecularReflectionBrdf(r, fresnel);
        }

        public override Color GetColor(Vector3 wo, Vector3 wi)
        {
            return brdf.Evaluate(wo, wi) * Color;
        }

        public override float Sample(in Surfel surfel, out Vector3 wi, out float pdf)
        {
            return brdf.Sample(surfel, out wi, RandomF.CreateSample(), out pdf);
        }
    }
}
