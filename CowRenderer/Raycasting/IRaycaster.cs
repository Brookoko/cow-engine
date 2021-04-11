namespace CowRenderer.Raycasting
{
    using CowLibrary;

    public interface IRaycaster
    {
        bool Raycast(Scene scene, Ray ray, out Surfel surfel);
    }
}