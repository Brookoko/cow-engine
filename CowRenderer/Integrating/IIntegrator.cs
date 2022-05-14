namespace CowRenderer
{
    using CowLibrary;

    public interface IIntegrator
    {
        Color GetColor(Scene scene, in Surfel surfel);
    }
}
