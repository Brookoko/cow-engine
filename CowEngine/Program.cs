namespace CowEngine
{
    using System;
    using System.Collections.Generic;
    using Cowject;
    using CowRenderer;
    using CpuRenderer;
    using ILGPURenderer;
    using ImageWorker;

    public class Program
    {
        private static readonly List<IModule> Modules = new()
        {
            new CoreModule(),
            new ImageModule(),
            new RendererModule(),
            new CpuModule(),
            new ILGPUModule(),
            new ObjModule(),
            new SceneModule()
        };

        public static void Main(string[] args)
        {
            var container = SetupContainer();

            var argumentParser = container.Get<IArgumentsParser>();

            argumentParser.Parse(args);
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to render. See next log for more details");
                Console.WriteLine(e);
            }
        }

        private static DiContainer SetupContainer()
        {
            var container = new DiContainer();
            foreach (var module in Modules)
            {
                module.Prepare(container);
            }
            return container;
        }
    }
}
