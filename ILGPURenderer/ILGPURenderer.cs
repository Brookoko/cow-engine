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

        public Image Render(Scene scene)
        {
            var sceneData = SceneConverter.Convert(scene);
            var camera = scene.MainCamera;
            var colors = RenderKernel.Render(sceneData, camera);
            return new Image(colors);
        }
    }
}
