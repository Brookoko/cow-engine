namespace CowLibrary.Lights
{
    using System;
    using System.Numerics;

    public class SphereLight : Light
    {
        public override Color Color { get; }
        
        private readonly float intensity;
        private readonly float radius;
        
        public SphereLight(Color color, float intensity, float radius)
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