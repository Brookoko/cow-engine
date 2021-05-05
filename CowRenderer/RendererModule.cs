namespace CowRenderer
{
    using Cowject;
    using Integration;
    using Raycasting;
    using Rendering;

    public class RendererModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<IIntegrator>().To<TracingIntegrator>().ToSingleton();
            container.Bind<IRaycaster>().To<SimpleRaycaster>().ToSingleton();
            container.Bind<IRenderer>().To<MultithreadRenderer>().ToSingleton();
            container.Bind<ThreadRenderer>().To<MultiRayThreadRenderer>();
            container.Bind<RenderConfig>().To<RenderConfig>().ToSingleton();
        }
    }
}