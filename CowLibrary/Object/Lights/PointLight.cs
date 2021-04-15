namespace CowLibrary.Lights
{
    using System;
    using System.Numerics;

    public class PointLight : Light
    {
        public override Color Color { get; }
        
        private readonly float intensity;
        private readonly float radius;
        
        public PointLight(Color color, float intensity, float radius)
        {
            Color = color;
            this.intensity = intensity;
            this.radius = radius;
        }
        
        public override Vector3 GetDirection(Vector3 point)
        {
            return (point - transform.position).Normalize();
        }
        
        public override float GetIntensity(Vector3 point)
        {
            var distance = (point - transform.position).Length();
            return Math.Max(1 - distance / radius, 0) * intensity;
        }
    }
}