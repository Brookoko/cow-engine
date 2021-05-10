namespace CowRenderer.Integration
{
    using System;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.InteropServices.ComTypes;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;

    public class MaterialIntegrator : IIntegrator
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

            var result = new Color(0f);
            foreach (var light in scene.lights)
            {
                result += GetLighting(surfel, light, 0);
            }
            return result;
        }

        private Color GetLighting(Surfel surfel, Light light, int depth)
        {
            return GetDirectLighting(surfel, light) + GetIndirectLighting(surfel, light, depth);
        }
        
        private Color GetDirectLighting(Surfel surfel, Light light)
        {
            var shading = light.GetShadingInfo(surfel.point);
            var color = surfel.material.GetColor(surfel.ray, shading.direction);
            var dot = Vector3.Dot(surfel.normal, shading.direction);
            dot = Math.Max(dot, 0);
            var multiplier = TraceShadowRay(surfel, shading.direction, shading.distance);
           return multiplier * color * shading.color * dot;
        }
        
        private Color GetIndirectLighting(Surfel surfel, Light light, int depth)
        {
            var result = new Color(0f);
            for (var i = 0; i < RenderConfig.numberOfIndirectRay; i++)
            {
                var f = surfel.material.Sample(surfel, out var wi, out var pdf);
                if (pdf > 0)
                {
                    result += f * pdf * Trace(surfel, light, wi, depth);
                }
            }
            return result / RenderConfig.numberOfIndirectRay;
        }
        
        private float TraceShadowRay(Surfel surfel, Vector3 direction, float distance)
        {
            var position = surfel.point + surfel.normal * RenderConfig.bias;
            var isHit = Raycaster.Raycast(new Ray(position, direction), out var hit);
            return isHit && hit.t < distance ? 0 : 1;
        }
        
        private Color Trace(Surfel surfel, Light light, Vector3 direction, int depth)
        {
            if (depth >= RenderConfig.rayDepth)
            {
                return new Color(0f);
            }
            var sign = Math.Sign(Vector3.Dot(surfel.normal, direction));
            var position = surfel.point + sign * surfel.normal * RenderConfig.bias;
            var isHit = Raycaster.Raycast(new Ray(position, direction), out var hit);
            return isHit ? GetLighting(hit, light, depth + 1) : new Color(0f);
        }
    }
}