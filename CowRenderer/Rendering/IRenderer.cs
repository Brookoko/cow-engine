namespace CowRenderer
{
    using CowLibrary;

    public interface IRenderer
    {
        Image Render(Scene scene);
    }
}