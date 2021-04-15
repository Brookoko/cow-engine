namespace CowEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            //model = new RenderableObject(new Sphere(new Vector3(0, 0, 0), 1f), new Material());
            scene.objects.Add(model);
            scene.PrepareScene();
            return scene;
        }

        private static Box GetBoundingBoxFor(List<RenderableObject> renderableObjects)
        {
            var min = renderableObjects.First().mesh.BoundingBox.min;
            var max = renderableObjects.First().mesh.BoundingBox.max;
            foreach (var renderableObject in renderableObjects)
            {
                var boundingBox = renderableObject.mesh.BoundingBox;
                min.X = Math.Min(min.X, boundingBox.min.X);
                min.Y = Math.Min(min.Y, boundingBox.min.Y);
                min.Z = Math.Min(min.Z, boundingBox.min.Z);
                max.X = Math.Max(max.X, boundingBox.max.X);
                max.Y = Math.Max(max.Y, boundingBox.max.Y);
                max.Z = Math.Max(max.Z, boundingBox.max.Z);
            }

            return new Box(min, max);
        }
    }
}