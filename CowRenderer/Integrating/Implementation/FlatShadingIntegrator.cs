namespace CowRenderer.Integration
{
    using System;
    using System.Numerics;
    using CowLibrary;

    public class FlatShadingIntegrator : IIntegrator
    {
        private readonly Color nullColor = new Color(245, 245, 245);
        
        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return nullColor;
            }

            var baseColor = surfel.material.color;
            var result = new Color(0, 0, 0);
            foreach (var light in scene.lights)
            {
                var direction = light.GetDirection(surfel.point);
                var dot = Vector3.Dot(-direction, surfel.normal);
                dot = Math.Max(dot, 0);
                var intensity = light.GetIntensity(surfel.point);
                var lightColor = light.Color * (intensity * dot);
                result += lightColor * baseColor;
            }
            return result;
        }
    }
}