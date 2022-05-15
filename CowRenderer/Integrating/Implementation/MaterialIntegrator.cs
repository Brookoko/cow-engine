namespace CowRenderer.Integration
{
    using System;
    using System.Diagnostics;
    using System.Linq;
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

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (surfel.material == null)
            {
                var environment = scene.lights.FirstOrDefault(l => l is EnvironmentLight);
                return environment?.Sample(in surfel.ray) ?? Color.Black;
            }

            var result = Color.Black;
            foreach (var light in scene.lights)
            {
                result += GetLighting(in surfel, light, 0);
            }
            return result;
        }

        private Color GetLighting(in Surfel surfel, Light light, int depth)
        {
            return GetDirectLighting(in surfel, light) + GetIndirectLighting(in surfel, light, depth);
        }

        private Color GetDirectLighting(in Surfel surfel, Light light)
        {
            var shading = light.GetShadingInfo(in surfel.hit);
            var color = surfel.material.GetColor(surfel.ray, shading.direction);
            var dot = Vector3.Dot(surfel.hit.normal, shading.direction);
            dot = Math.Max(dot, 0);
            var multiplier = TraceShadowRay(in surfel, shading.direction, shading.distance);
            return multiplier * color * shading.color * dot;
        }

        private Color GetIndirectLighting(in Surfel surfel, Light light, int depth)
        {
            var result = Color.Black;
            var n = RenderConfig.numberOfRayPerLight;
            for (var i = 0; i < n; i++)
            {
                var f = surfel.material.Sample(in surfel.hit.normal, in surfel.ray, out var wi, out var pdf);
                if (pdf > 0 && f > 0)
                {
                    result += f * pdf * Trace(in surfel, light, wi, depth);
                }
            }
            return result / RenderConfig.numberOfRayPerLight;
        }

        private float TraceShadowRay(in Surfel surfel, Vector3 direction, float distance)
        {
            var position = surfel.hit.point + surfel.hit.normal * RenderConfig.bias;
            var isHit = Raycaster.Raycast(new Ray(position, direction), out var surfelHit);
            return isHit && surfelHit.hit.t < distance ? 0 : 1;
        }

        private Color Trace(in Surfel surfel, Light light, Vector3 direction, int depth)
        {
            if (depth >= RenderConfig.rayDepth)
            {
                return Color.Black;
            }
            var sign = Math.Sign(Vector3.Dot(surfel.hit.normal, direction));
            var position = surfel.hit.point + sign * surfel.hit.normal * RenderConfig.bias;
            if (Raycaster.Raycast(new Ray(position, direction), out var hit))
            {
                return GetLighting(in hit, light, depth + 1);
            }
            var lightning = light.Sample(in direction);
            var dot = Vector3.Dot(surfel.hit.normal, direction);
            dot = Math.Max(dot, 0);
            return surfel.material.Color * lightning * dot;
        }
    }
}
