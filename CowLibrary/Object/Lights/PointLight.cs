namespace CowLibrary.Lights
{
    using System.Numerics;

    public class PointLight : Light
    {
        public override Color Color { get; }
        
        private readonly float intensity;
        
        public PointLight(Color color, float intensity)
        {
            Color = color;
            this.intensity = intensity;
        }
        
        public override Vector3 GetDirection(Vector3 point)
        {
            return (point - transform.position).Normalize();
        }
        
        public override float GetIntensity(Vector3 point)
        {
            return intensity;
        }
    }
}