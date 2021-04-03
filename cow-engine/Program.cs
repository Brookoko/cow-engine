namespace CowEngine
{
    using System;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Structure;
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
                var model = objWorker.Parse(source);
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
            container.Bind<IObjWorker>().To<ObjWorker>().ToSingleton();
            container.Bind<IRenderer>().To<DummyRenderer>().ToSingleton();
            container.Bind<IIntegrator>().To<DummyIntegrator>().ToSingleton();
            
            var imageModule = new ImageModule();
            imageModule.Prepare(container);
            
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
