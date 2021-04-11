namespace CowEngine
{
    using CowLibrary;
    using CowLibrary.Structure;

    public class DummyRaycaster : IRaycaster
    {
        public bool Raycast(Scene scene, Ray ray, out Surfel surfel)
        {
            surfel = null;
            return false;
        }
    }
}