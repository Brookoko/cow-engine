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

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (surfel.material == null)
            {
                return backgroundColor;
            }

            var baseColor = surfel.material.Color;
            var result = new Color(0, 0, 0);
            foreach (var light in scene.lights)
            {
                var shading = light.GetShadingInfo(in surfel.hit);
                var dot = Vector3.Dot(shading.direction, surfel.hit.normal);
                dot = Math.Max(dot, 0);
                var multiplier = TraceShadowRay(in surfel, in shading.direction, shading.distance);
                result += multiplier * baseColor * shading.color * dot;
            }
            return result;
        }

        private float TraceShadowRay(in Surfel surfel, in Vector3 direction, float distance)
        {
            var position = surfel.hit.point + surfel.hit.normal * RenderConfig.bias;
            var isHit = Raycaster.Raycast(new Ray(position, direction), out var surfelHit);
            return isHit && surfelHit.hit.t < distance ? 0 : 1;
        }
    }
}
