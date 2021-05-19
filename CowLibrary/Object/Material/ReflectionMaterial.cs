namespace CowLibrary
{
    using System.Numerics;

    public class ReflectionMaterial : Material
    {
        private BRDF brdf;
        
        public ReflectionMaterial(float r, float eta) : base(Color.White)
        {
            var fresnel = new DielectricFresnel(1, eta);
            brdf = new SpecularReflectionBRDF(r, fresnel);
        }

        public override Color GetColor(Vector3 wo, Vector3 wi)
        {
            return brdf.Evaluate(wo, wi) * color;
        }
        
        public override float Sample(Surfel surfel, out Vector3 wi, out float pdf)
        {
            return brdf.Sample(surfel, out wi, Mathf.CreateSample(), out pdf);
        }
    }
}