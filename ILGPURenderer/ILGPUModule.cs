namespace ILGPURenderer
{
    using Cowject;
    using CowRenderer;

    public class ILGPUModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            var gpuKernel = new GpuKernel(KernelMode.Gpu);
            container.Bind<GpuKernel>().ToInstance(gpuKernel);
            container.Bind<IRenderer>().WithName(KernelMode.Gpu).To<ILGPURenderer>().ToSingleton();
            container.Bind<IPrimaryRayGenerator>().To<PrimaryRayGenerator>().ToSingleton();
            container.Bind<IHitGenerator>().To<HitGenerator>().ToSingleton();
            container.Bind<ILocalSamplerProvider>().To<LocalSamplerProvider>().ToSingleton();
        }
    }
}
