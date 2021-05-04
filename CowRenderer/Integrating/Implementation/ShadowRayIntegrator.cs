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
                var shading = light.GetShadingInfo(surfel.point);
                var dot = Vector3.Dot(shading.direction, surfel.normal);
                dot = Math.Max(dot, 0);
                var multiplier = TraceShadowRay(surfel, shading.direction, shading.distance);
                result += multiplier * baseColor * shading.color * dot;
            }
            return result;
        }

        private float TraceShadowRay(Surfel surfel, Vector3 direction, float distance)
        {
            var position = surfel.point + surfel.normal * RenderConfig.bias;
            var isHit = Raycaster.Raycast(new Ray(position, direction), out var hit);
            return isHit && hit.t < distance ? 0 : 1;
        }
    }
}