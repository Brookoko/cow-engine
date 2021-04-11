namespace CowRenderer.Raycasting.Impl
{
    using CowLibrary;

    public class DummyRaycaster : IRaycaster
    {
        public bool Raycast(Scene scene, Ray ray, out Surfel surfel)
        {
            surfel = null;
            return false;
        }
    }
}