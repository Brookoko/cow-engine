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
        
        public override ShadingInfo GetShadingInfo(Vector3 point)
        {
            return new ShadingInfo()
            {
                direction = Mathf.OnUnitSphere(),
                distance = float.PositiveInfinity,
                color = color * intensity
            };
        }
        
        public override Color Sample(Surfel surfel, Vector3 wi)
        {
            var dot = Vector3.Dot(surfel.normal, wi);
            dot = Math.Max(dot, 0);
            return surfel.material.color * color * intensity * dot;
        }
    }
}