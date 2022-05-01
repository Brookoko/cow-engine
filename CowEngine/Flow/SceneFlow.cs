namespace CowEngine
{
    using Cowject;
    using CowRenderer;
    using ImageWorker;
    using SceneWorker;

    public class SceneFlow : IFlow
    {
        [Inject]
        public ISceneWorker SceneWorker { get; set; }

        [Inject]
        public IRenderer Renderer { get; set; }

        [Inject]
        public IImageWorker ImageWorker { get; set; }

        [Inject]
        public IWatch Watch { get; set; }

        public int Process(string source, string output)
        {
            Watch.Start();
            var scene = SceneWorker.Parse(source);
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
