namespace CowRenderer.Integration
{
    using System;
    using System.Numerics;
    using Cowject;
    using CowLibrary;

    public class ShadowRayIntegrator : IIntegrator
    {
        [Inject]
        public IRaycaster Raycaster { get; set; }
        
        [Inject]
        public RenderConfig RenderConfig { get; set; }
        
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
                var direction = -light.GetDirection(surfel.point);
                var dot = Vector3.Dot(direction, surfel.normal);
                dot = Math.Max(dot, 0);
                
                var intensity = light.GetIntensity(surfel.point);
                var lightColor = intensity * dot * light.Color;
                var multiplier = TraceShadowRay(scene, surfel, direction);
                
                result += multiplier * lightColor * baseColor;
            }
            return result;
        }

        private float TraceShadowRay(Scene scene, Surfel surfel, Vector3 direction)
        {
            var position = surfel.point + surfel.normal * RenderConfig.bias;
            var isHit = Raycaster.Raycast(scene, new Ray(position, direction), out var hit);
            return isHit ? 0 : 1;
        }
    }
}