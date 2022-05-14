namespace CowEngine
{
    using Cowject;
    using CowRenderer;
    using ImageWorker;

    public class CpuFlow : IFlow<CpuOption>
    {
        [Inject(Name = KernelMode.Cpu)]
        public IRenderer Renderer { get; set; }

        [Inject]
        public IImageWorker ImageWorker { get; set; }

        [Inject]
        public IWatch Watch { get; set; }

        [Inject]
        public ISceneLoader SceneLoader { get; set; }
        
        public int Process(CpuOption option)
        {
            Watch.Start();
            var scene = SceneLoader.LoadSceneFromOptions(option);
            Watch.Stop("Loading scene");

            Watch.Start();
            scene.PrepareScene();
            Watch.Stop("Preparing scene");

            Watch.Start();
            var image = Renderer.Render(scene);
            Watch.Stop("Rendering scene");

            Watch.Start();
            ImageWorker.SaveImage(in image, option.Output);
            Watch.Stop("Saving render");

            return 0;
        }
    }
}
