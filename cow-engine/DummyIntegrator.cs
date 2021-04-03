namespace CowEngine
{
    using CowLibrary;
    using CowLibrary.Structure;

    public class DummyIntegrator : IIntegrator
    {
        public Color GetColor(Scene scene, Surfel surfel)
        {
            return new Color(255, 255, 255);
        }
    }
}