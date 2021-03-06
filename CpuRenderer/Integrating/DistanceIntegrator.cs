namespace CowRenderer.Integration
{
    using CowLibrary;

    public class DistanceIntegrator : IIntegrator
    {
        private readonly Color negativeColor = Color.White;

        private readonly Color minColor = Color.Red;

        private readonly float minDistance = 0f;

        private readonly Color maxColor = Color.Blue;

        private readonly float maxDistance = 2f;

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (!surfel.hit.HasHit)
            {
                return negativeColor;
            }

            return ColorExtensions.LerpUnclamped(minColor, maxColor,
                (surfel.hit.t - minDistance) / (maxDistance - minDistance));
        }
    }
}
