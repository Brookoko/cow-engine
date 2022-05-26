namespace CowRenderer.Integration
{
    using System;
    using System.Numerics;
    using CowLibrary;

    public class FlatShadingIntegrator : IIntegrator
    {
        private readonly Color backgroundColor = new Color(245, 245, 245);

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (!surfel.hit.HasHit)
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
                result += baseColor * shading.color * dot;
            }
            return result;
        }
    }
}
