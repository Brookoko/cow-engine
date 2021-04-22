namespace CowEngine
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;
    using CowRenderer;
    using ImageWorker;

    public class Program
    {
        private static readonly Watch watch = new Watch();
        
        private static readonly List<IModule> modules = new List<IModule>()
        {
            new ImageModule(),
            new RendererModule(),
            new ObjModule()
        };
        
        public static void Main(string[] args)
        {
            var container = SetupContainer();

            var argumentParser = container.Get<IArgumentsParser>();
            var objWorker = container.Get<IRenderableObjectWorker>();
            var renderer = container.Get<IRenderer>();
            var imageWorker = container.Get<IImageWorker>();

            try
            {
                var (source, output) = argumentParser.Parse(args);
                
                watch.Start();
                var model = objWorker.Parse(source);
                watch.Stop("Loading model");
                
                watch.Start();
                var scene = PrepareScene(container, model);
                watch.Stop("Preparing scene");
                
                watch.Start();
                var image = renderer.Render(scene);
                watch.Stop("Rendering scene");
                
                watch.Start();
                imageWorker.SaveImage(image, output);
                watch.Stop("Saving render");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to render model. See next log for more detail");
                Console.WriteLine(e);
                throw;
            }
        }

        private static DiContainer SetupContainer()
        {
            var container = new DiContainer();
            container.Bind<IArgumentsParser>().To<ArgumentsParser>().ToSingleton();
            container.Bind<IIoWorker>().To<IoWorker>().ToSingleton();
            foreach (var module in modules)
            {
                module.Prepare(container);
            }
            
            return container;
        }

        private static Scene PrepareScene(DiContainer container, RenderableObject model)
        {
            var scene = new Scene();
            container.Inject(scene);
            
            var light1 = new PointLight(new Color(135, 15, 220), 1f, 80);
            light1.transform.position = new Vector3(10, 20, 25);
            scene.lights.Add(light1);
            
            var light2 = new PointLight(new Color(135, 15, 220), 1f, 80);
            light2.transform.position = new Vector3(-10, -20, -25);
            scene.lights.Add(light2);
            
            scene.objects.Add(model);
            scene.PrepareScene();
            return scene;
        }
    }
}