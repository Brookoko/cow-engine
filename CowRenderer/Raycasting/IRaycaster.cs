namespace CowRenderer
{
    using CowLibrary;

    public interface IRaycaster
    {
        void Init(Scene scene);

        bool Raycast(Ray ray, out Surfel surfel);
    }
}
