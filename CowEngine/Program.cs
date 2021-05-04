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
    using SceneWorker;

    public class Program
    {
        private static readonly List<IModule> modules = new List<IModule>()
        {
            new ImageModule(),
            new RendererModule(),
            new ObjModule(),
            new SceneModule()
        };
        
        public static void Main(string[] args)
        {
            var container = SetupContainer();

            var argumentParser = container.Get<IArgumentsParser>();
            
            try
            {
                argumentParser.Parse(args,
                    opts => RenderCompiledScene(container, opts.Output),
                    opts => RenderModel(container, opts.Source, opts.Output),
                    opts => RenderScene(container, opts.Source, opts.Output)
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to render model. See next log for more details");
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

        private static int RenderScene(DiContainer container, string source, string output)
        {
            var watch = new Watch();
            var sceneWorker = container.Get<ISceneWorker>();
            var renderer = container.Get<IRenderer>();
            var imageWorker = container.Get<IImageWorker>();
            
            watch.Start();
            var scene = sceneWorker.Parse(source);
            watch.Stop("Loading scene");
            
            watch.Start();
            scene.PrepareScene();
            watch.Stop("Preparing scene");
            
            watch.Start();
            var image = renderer.Render(scene);
            watch.Stop("Rendering scene");
            
            watch.Start();
            imageWorker.SaveImage(image, output);
            watch.Stop("Saving render");
            
            return 0;
        }
        
        private static int RenderCompiledScene(DiContainer container, string output)
        {
            var watch = new Watch();
            var renderer = container.Get<IRenderer>();
            var imageWorker = container.Get<IImageWorker>();
            
            watch.Start();
            var scene = new CompiledScene();
            container.Inject(scene);
            scene.PrepareScene();
            watch.Stop("Preparing scene");
            
            watch.Start();
            var image = renderer.Render(scene);
            watch.Stop("Rendering scene");
            
            watch.Start();
            imageWorker.SaveImage(image, output);
            watch.Stop("Saving render");
            
            return 0;
        }

        private static int RenderModel(DiContainer container, string source, string output)
        {
            var watch = new Watch();
            var objWorker = container.Get<IRenderableObjectWorker>();
            var renderer = container.Get<IRenderer>();
            var imageWorker = container.Get<IImageWorker>();
            
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
            
            return 0;
        }
        
        private static Scene PrepareScene(DiContainer container, RenderableObject model)
        {
            var scene = new AutoAdjustScene();
            container.Inject(scene);
            
            var light = new PointLight(new Color(255, 255, 255), 100);
            light.transform.position = new Vector3(0, 0, 5f);
            scene.lights.Add(light);
            
            scene.objects.Add(model);
            scene.PrepareScene();
            return scene;
        }
    }
}