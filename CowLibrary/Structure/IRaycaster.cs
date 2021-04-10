namespace CowLibrary.Structure
{
    public interface IRaycaster
    {
        bool Raycast(Scene scene, Ray ray, out Surfel surfel);
    }
}