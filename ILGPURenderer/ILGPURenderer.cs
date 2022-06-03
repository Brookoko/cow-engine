namespace ILGPURenderer
{
    using Converters;
    using Cowject;
    using CowLibrary;
    using CowRenderer;

    public class ILGPURenderer : IRenderer
    {
        [Inject]
        public IRenderKernel RenderKernel { get; set; }

        [Inject]
        public ISceneConverter SceneConverter { get; set; }

        public string Tag => "ilgpu";

        public void Prepare(Scene scene)
        {
            RenderKernel.Prepare(scene);
        }

        public Image Render(Scene scene)
        {
            var camera = scene.MainCamera;
            var colors = RenderKernel.Render(camera);
            return new Image(colors);
        }
    }
}
