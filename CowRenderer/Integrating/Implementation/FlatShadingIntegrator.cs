namespace CowRenderer.Integration
{
    using System;
    using System.Numerics;
    using CowLibrary;

    public class FlatShadingIntegrator : IIntegrator
    {
        private readonly Color backgroundColor = new Color(245, 245, 245);
        
        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return backgroundColor;
            }

            var baseColor = surfel.material.color;
            var result = new Color(0, 0, 0);
            foreach (var light in scene.lights)
            {
                var shading = light.GetShadingInfo(surfel.point);
                var dot = Vector3.Dot(shading.direction, surfel.normal);
                dot = Math.Max(dot, 0);
                result += baseColor * shading.color * dot;
            }
            return result;
        }
    }
}