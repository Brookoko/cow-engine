namespace CowRenderer
{
    using CowLibrary;

    public interface IRenderer
    {
        string Tag { get; }

        Image Render(Scene scene);
    }
}
