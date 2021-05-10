namespace CowLibrary
{
    using System.Numerics;

    public class DiffuseMaterial : Material
    {
        private readonly BRDF brdf;
        
        public DiffuseMaterial(Color color, float r) : base(color)
        {
            brdf = new LambertianBRDF(r);
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