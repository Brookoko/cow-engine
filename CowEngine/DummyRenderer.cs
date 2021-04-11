namespace CowEngine
{
    using Cowject;
    using CowLibrary;
    using CowLibrary.Structure;

    public class DummyRenderer : IRenderer
    {
        [Inject]
        public IRaycaster Raycaster { get; set; }
        
        [Inject]
        public IIntegrator Integrator { get; set; }
        
        public Image Render(Scene scene)
        {
            return new Image(128, 128);
        }
    }
}