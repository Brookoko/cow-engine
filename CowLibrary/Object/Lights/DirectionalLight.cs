namespace CowLibrary.Lights
{
    using System.Numerics;

    public class DirectionalLight : Light
    {
        private readonly Color color;
        private readonly float intensity;
        
        public DirectionalLight(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }
        
        public override ShadingInfo GetShadingInfo(Vector3 point)
        {
            return new ShadingInfo()
            {
                direction = -transform.forward,
                distance = float.PositiveInfinity,
                color = color * intensity
            };
        }
    }
}