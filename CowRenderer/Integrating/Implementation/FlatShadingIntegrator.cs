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
            var luminosity = 0f;
            foreach (var light in scene.lights)
            {
                var direction = light.GetDirection(surfel.point);
                var dotProduct = Vector3.Dot(direction, surfel.normal);
                dotProduct = Math.Max(dotProduct, 0);
                var intensity =  light.GetIntensity(surfel.point);
                var lightLuminosity = dotProduct * intensity;
                luminosity += lightLuminosity;
            }
            
            return baseColor * luminosity;
        }
    }
}