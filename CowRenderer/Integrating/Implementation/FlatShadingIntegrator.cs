namespace CowRenderer.Integration.Impl
{
    using System;
    using CowLibrary;

    public class FlatShadingIntegrator : IIntegrator
    {
        private readonly Color black = new Color(0, 0, 0);
        private readonly Color white = new Color(255, 255, 255);

        private readonly Color nullColor = new Color(210, 210, 210);
        
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
                var angle = surfel.normal.AngleRadTo(-direction);
                angle = Math.Min(Math.Max(angle, 0), Math.PI / 2);
                var intensity =  light.GetIntensity(surfel.point);
                luminosity += (float) Math.Cos(angle) * intensity;
            }

            return baseColor * luminosity;
        }
    }
}