namespace CowEngine
{
    using System;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowRenderer;
    using CowRenderer.Integration.Impl;
    using CowRenderer.Raycasting.Impl;
    using CowRenderer.Rendering.Impl;
    using ImageWorker;
    
    public class Program
    {
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
                var model = new RenderableObject(new Sphere(Vector3.Zero, 1), new Material()); //objWorker.Parse(source);
                var scene = PrepareScene(model);
                var image = renderer.Render(scene);
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
            container.Bind<IRaycaster>().To<SimpleRaycaster>().ToSingleton();
            container.Bind<IRenderer>().To<SimpleRenderer>().ToSingleton();
            container.Bind<IIntegrator>().To<BwIntegrator>().ToSingleton();
            
            var imageModule = new ImageModule();
            imageModule.Prepare(container);
            var objModule = new ObjModule();
            objModule.Prepare(container);
            
            return container;
        }
        
        private static Scene PrepareScene(RenderableObject model)
        {
            var scene = new Scene();
            scene.objects.Add(model);
            return scene;
        }
    }
}
