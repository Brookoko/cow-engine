namespace ILGPURenderer
{
    using Cowject;
    using CowRenderer;
    using CowRenderer.Rendering;

    public class ILGPUModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<IRenderer>().WithName(KernelMode.Gpu).To<ILGPURenderer>().ToSingleton();
        }
    }
}
