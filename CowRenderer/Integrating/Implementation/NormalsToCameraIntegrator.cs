namespace CowRenderer.Integration
{
    using CowLibrary;

    public class NormalsToCameraIntegrator : IIntegrator
    {
        private readonly Color nullColor = Color.White;

        private readonly (Color, Color) negativeColors = (new Color(255, 0, 0), new Color(0, 0, 0));

        private readonly (Color, Color) lerpingColors = (new Color(20, 207, 220), new Color(15, 50, 210));

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (!surfel.hasHit)
            {
                return nullColor;
            }

            var camera = scene.MainCamera;
            var surfelToCameraDirection = camera.Transform.Position - surfel.hit.point;
            var angle = surfelToCameraDirection.AngleTo(surfel.hit.normal);
            return angle <= 90
                ? ColorExtensions.LerpUnclamped(lerpingColors.Item1, lerpingColors.Item2, (float)angle / 90)
                : ColorExtensions.LerpUnclamped(negativeColors.Item1, negativeColors.Item2, (float)(angle - 90) / 90);
        }
    }
}
