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
    using CowLibrary.Mathematics.Sampler;

    public class MaterialIntegrator : IIntegrator
    {
        [Inject]
        public IRaycaster Raycaster { get; set; }

        [Inject]
        public RenderConfig RenderConfig { get; set; }

        [Inject]
        public ISamplerProvider SamplerProvider { get; set; }
        
        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (!surfel.hit.HasHit)
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
            var basis = surfel.hit.ExtractBasis();
            var wo = basis.WorldToLocal(in surfel.ray);
            var wi = basis.WorldToLocal(in shading.direction);
            var color = surfel.material.GetColor(in wo, in wi);
            var dot = Vector3.Dot(surfel.hit.normal, shading.direction);
            dot = Math.Max(dot, 0);
            var multiplier = TraceShadowRay(in surfel, shading.direction, shading.distance);
            return multiplier * color * shading.color * dot;
        }

        private Color GetIndirectLighting(in Surfel surfel, Light light, int depth)
        {
            var result = Color.Black;
            var n = RenderConfig.numberOfRayPerMaterial;
            for (var i = 0; i < n; i++)
            {
                var sample = SamplerProvider.Sampler.CreateSample();
                var basis = surfel.hit.ExtractBasis();
                var wo = basis.WorldToLocal(in surfel.ray);
                var f = surfel.material.Sample(in wo, in sample, out var wi, out var pdf);
                if (pdf > 0 && f != Color.Black)
                {
                    result += f * Trace(in surfel, light, wi, depth) / pdf;
                }
            }
            return result / RenderConfig.numberOfRayPerMaterial;
        }

        private float TraceShadowRay(in Surfel surfel, Vector3 direction, float distance)
        {
            var position = surfel.hit.point + surfel.hit.normal * Const.Bias;
            var surfelHit = Raycaster.Raycast(new Ray(position, direction));
            return surfelHit.hit.HasHit && surfelHit.hit.t < distance ? 0 : 1;
        }

        private Color Trace(in Surfel surfel, Light light, Vector3 direction, int depth)
        {
            if (depth >= RenderConfig.rayDepth)
            {
                return Color.Black;
            }
            var sign = Math.Sign(Vector3.Dot(surfel.hit.normal, direction));
            var position = surfel.hit.point + sign * surfel.hit.normal * Const.Bias;
            var surfelHit = Raycaster.Raycast(new Ray(position, direction));
            if (surfelHit.hit.HasHit)
            {
                return GetIndirectLighting(in surfelHit, light, depth + 1);
            }
            var lightning = light.Sample(in direction);
            var dot = Vector3.Dot(surfel.hit.normal, direction);
            dot = Math.Max(dot, 0);
            return lightning * dot;
        }
    }
}
