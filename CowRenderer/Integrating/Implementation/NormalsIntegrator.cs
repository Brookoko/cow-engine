namespace CowRenderer.Integration.Impl
{
    using System.Numerics;
    using CowLibrary;

    public class NormalsIntegrator : IIntegrator
    {
        private readonly Color negativeColor = new Color(255, 255, 255);

        private readonly (Color, Color) lerpingColors = (new Color(255,0,0), new Color(0,0,250));

        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return negativeColor;
            }

            var normal = surfel.normal;
            normal = normal / 2f + Vector3.One / 2f;
            return Color.LerpUnclamped(lerpingColors.Item1, lerpingColors.Item2, normal);
        }
    }
}