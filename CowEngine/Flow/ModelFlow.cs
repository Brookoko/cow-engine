namespace CowEngine
{
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;
    using CowRenderer;
    using ImageWorker;

    public class ModelFlow : IFlow
    {
        [Inject]
        public DiContainer DiContainer { get; set; }
        
        [Inject]
        public IRenderableObjectWorker RenderableObjectWorker { get; set; }
        
        [Inject]
        public IRenderer Renderer { get; set; }

        [Inject]
        public IImageWorker ImageWorker { get; set; }
        
        [Inject]
        public IWatch Watch { get; set; }
        
        public int Process(string source, string output)
        {
            Watch.Start();
            var model = RenderableObjectWorker.Parse(source);
            Watch.Stop("Loading model");
            
            Watch.Start();
            var scene = PrepareScene(model);
            Watch.Stop("Preparing scene");
            
            Watch.Start();
            var image = Renderer.Render(scene);
            Watch.Stop("Rendering scene");
            
            Watch.Start();
            ImageWorker.SaveImage(image, output);
            Watch.Stop("Saving render");
            
            return 0;
        }
        
        private Scene PrepareScene(RenderableObject model)
        {
            var scene = new AutoAdjustScene();
            DiContainer.Inject(scene);
            
            var light = new EnvironmentLight(new Color(255, 255, 255), 8);
            scene.lights.Add(light);
            
            scene.objects.Add(model);
            scene.PrepareScene();
            return scene;
        }
    }
}