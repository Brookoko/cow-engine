namespace CowRenderer
{
    using CowLibrary;

    public interface IRenderer
    {
        string Tag { get; }

        void Prepare(Scene scene);
        
        Image Render(Scene scene);
    }
}
