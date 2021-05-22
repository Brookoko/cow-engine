namespace CowLibrary.Lights
{
    using System;
    using System.Numerics;

    public class EnvironmentLight : Light
    {
        private readonly Color color;
        private readonly float intensity;
        
        public EnvironmentLight(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }
        
        public override ShadingInfo GetShadingInfo(Surfel surfel)
        {
            return new ShadingInfo()
            {
                direction = Mathf.CosineSampleHemisphere(surfel.normal, Mathf.CreateSample()),
                distance = float.PositiveInfinity,
                color = color * intensity
            };
        }
        
        public override Color Sample(Vector3 wi)
        {
            return color * intensity;
        }
    }
}