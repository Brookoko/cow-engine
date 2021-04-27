namespace CowRenderer.Integration
{
    using CowLibrary;

    public class DistanceIntegrator : IIntegrator
    {
        private readonly Color negativeColor = new Color(255, 255, 255);
        
        private readonly Color minColor = new Color(255, 0,0);

        private readonly float minDistance = 0f;
        
        private readonly Color maxColor = new Color(0, 0, 255);

        private readonly float maxDistance = 2f;

        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return negativeColor;
            }

            return ColorExtensions.LerpUnclamped(minColor, maxColor, (surfel.t - minDistance) / (maxDistance - minDistance));
        }
    }
}