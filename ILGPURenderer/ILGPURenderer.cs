namespace ILGPURenderer
{
    using Cowject;
    using CowLibrary;
    using CowRenderer;

    public class ILGPURenderer : IRenderer
    {
        [Inject]
        public IPrimaryRayGenerator PrimaryRayGenerator { get; set; }

        public Image Render(Scene scene)
        {
            var camera = scene.MainCamera;
            var w = camera.Width;
            var h = camera.Height;
            var image = new Image(w, h);
            var rays = PrimaryRayGenerator.GeneratePrimaryRays(camera);
            return image;
        }
    }
}
