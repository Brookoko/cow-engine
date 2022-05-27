namespace CowRenderer
{
    using Cowject;
    using CowLibrary.Mathematics.Sampler;

    public class RendererModule : IModule
    {
        public Priority Priority => Priority.High;

        public void Prepare(DiContainer container)
        {
            container.Bind<ISamplerProvider>().To<SamplerProvider>().ToSingleton();
            container.Bind<RenderConfig>().To<RenderConfig>().ToSingleton();
        }
    }
}
