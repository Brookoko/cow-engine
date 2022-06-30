namespace CowRenderer.Integration;

using CowLibrary;

public class TestIntegrator : IIntegrator
{
    public Color GetColor(Scene scene, in Surfel surfel)
    {
        return surfel.hit.HasHit ? Color.White : Color.Black;
    }
}
