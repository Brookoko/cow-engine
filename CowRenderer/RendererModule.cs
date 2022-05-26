namespace CowRenderer
{
    using Cowject;
    using CowLibrary.Mathematics.Sampler;
    using Integration;
    using Raycasting;
    using Rendering;

    public class RendererModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<ISamplerProvider>().To<SamplerProvider>().ToSingleton();
            container.Bind<IIntegrator>().To<MaterialIntegrator>().ToSingleton();
            container.Bind<IRaycaster>().To<SimpleRaycaster>().ToSingleton();
            container.Bind<ThreadRenderer>().To<MultiRayThreadRenderer>();
            container.Bind<RenderConfig>().To<RenderConfig>().ToSingleton();
            
            container.BindInterfacesTo<SimpleRenderer>().ToSingleton();
            container.BindInterfacesTo<MultithreadRenderer>().ToSingleton();
        }
    }
}
