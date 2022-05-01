namespace CowEngine
{
    using Cowject;
    using CowRenderer;
    using ImageWorker;

    public class CompiledFlow : IFlow
    {
        [Inject]
        public DiContainer DiContainer { get; set; }

        [Inject]
        public IRenderer Renderer { get; set; }

        [Inject]
        public IImageWorker ImageWorker { get; set; }

        [Inject]
        public IWatch Watch { get; set; }

        public int Process(string source, string output)
        {
            Watch.Start();
            var scene = new CompiledScene();
            DiContainer.Inject(scene);
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
