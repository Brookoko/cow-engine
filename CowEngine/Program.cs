namespace CowEngine
{
    using System;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;
    using CowRenderer;
    using ImageWorker;

    public class Program
    {
        private static Watch watch = new Watch();
        
        public static void Main(string[] args)
        {
            var container = SetupContainer();

            var argumentParser = container.Get<IArgumentsParser>();
            var objWorker = container.Get<IObjWorker>();
            var renderer = container.Get<IRenderer>();
            var imageWorker = container.Get<IImageWorker>();

            try
            {
                var (source, output) = argumentParser.Parse(args);
                var model = objWorker.Parse(source);
                var scene = PrepareScene(model);
                
                watch.Start();
                var image = renderer.Render(scene);
                watch.Stop("Render");
                
                imageWorker.SaveImage(image, output);
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

            var imageModule = new ImageModule();
            imageModule.Prepare(container);
            var rendererModule = new RendererModule();
            rendererModule.Prepare(container);
            var objModule = new ObjModule();
            objModule.Prepare(container);

            return container;
        }

        private static Scene PrepareScene(RenderableObject model)
        {
            var scene = new Scene();
            var light = new PointLight(new Color(20, 100, 200), 1f, 16);
            light.transform.position = new Vector3(0, 2, 0);
            
            scene.lights.Add(light);
            scene.objects.Add(model);
            scene.PrepareScene();
            return scene;
        }
    }
}