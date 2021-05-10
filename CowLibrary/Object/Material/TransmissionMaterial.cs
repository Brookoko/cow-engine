namespace CowLibrary
{
    using System.Numerics;

    public class TransmissionMaterial : Material
    {
        private BRDF brdf;
        
        public TransmissionMaterial(Color color, float t, float eta) : base(color)
        {
            brdf = new SpecularTransmissionBRDF(t, 1f, eta, TransportMode.Importance);
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