namespace CowLibrary
{
    using System.Numerics;

    public class FresnelMaterial : Material
    {
        private readonly BRDF brdf;
        
        public FresnelMaterial(float r, float t, float eta) : base(Color.White)
        {
            brdf = new FresnelSpecularBRDF(r, t, 1, eta, TransportMode.Importance);
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