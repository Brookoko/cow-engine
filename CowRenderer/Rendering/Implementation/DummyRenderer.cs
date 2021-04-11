namespace CowRenderer.Rendering.Impl
{
    using CowLibrary;

    public class DummyRenderer : IRenderer
    {
        public Image Render(Scene scene)
        {
            return new Image(128, 128);
        }
    }
}