namespace CowRenderer.Rendering
{
    using CowLibrary;

    public interface IRenderer
    {
        Image Render(Scene scene);
    }
}