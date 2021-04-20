namespace CowRenderer
{
    using Cowject;
    using Integration.Impl;
    using Raycasting.Impl;
    using Rendering.Impl;

    public class RendererModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<IIntegrator>().To<FlatShadingIntegrator>().ToSingleton();
            container.Bind<IRaycaster>().To<SimpleRaycaster>().ToSingleton();
            container.Bind<IRenderer>().To<MultithreadRenderer>().ToSingleton();
        }
    }
}