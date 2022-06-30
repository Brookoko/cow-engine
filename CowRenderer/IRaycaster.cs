namespace CowRenderer
{
    using CowLibrary;

    public interface IRaycaster
    {
        void Init(Scene scene);

        Surfel Raycast(in Ray ray);
    }
}
