namespace CowEngine
{
    using Cowject;
    using CowRenderer;
    using ImageWorker;

    public class SceneFlow : IFlow
    {
        [Inject]
        public IRenderer Renderer { get; set; }

        [Inject]
        public IImageWorker ImageWorker { get; set; }

        [Inject]
        public IWatch Watch { get; set; }

        public int Process(string source, string output)
        {
            Watch.Start();
            var scene = new AutoAdjustScene();
            Watch.Stop("Loading scene");

            Watch.Start();
            scene.PrepareScene();
            Watch.Stop("Preparing scene");

            Watch.Start();
            var image = Renderer.Render(scene);
            Watch.Stop("Rendering scene");

            Watch.Start();
            ImageWorker.SaveImage(image, output);
            Watch.Stop("Saving render");

            return 0;
        }
    }
}
