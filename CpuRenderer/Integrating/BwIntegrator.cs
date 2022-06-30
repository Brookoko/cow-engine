namespace CowRenderer.Integration
{
    using CowLibrary;

    public class BwIntegrator : IIntegrator
    {
        private readonly Color bColor = new Color(255, 255, 255);
        private readonly Color wColor = new Color(100, 100, 100);

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            return surfel.hit.HasHit ? wColor : bColor;
        }
    }
}
