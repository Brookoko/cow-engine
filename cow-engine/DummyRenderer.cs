namespace CowEngine
{
    using CowLibrary;
    using CowLibrary.Structure;

    public class DummyRenderer : IRenderer
    {
        public Image Render(Scene scene)
        {
            return new Image(128, 128);
        }
    }
}