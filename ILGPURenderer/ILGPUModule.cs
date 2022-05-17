namespace ILGPURenderer
{
    using Converters;
    using Cowject;
    using CowRenderer;

    public class ILGPUModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            var gpuKernel = new GpuKernel(KernelMode.Gpu);
            container.Bind<GpuKernel>().ToInstance(gpuKernel);
            container.Bind<IRenderer>().WithName(KernelMode.Gpu).To<ILGPURenderer>().ToSingleton();
            container.Bind<IPrimaryRayKernel>().To<PrimaryRayKernel>().ToSingleton();
            container.Bind<IHitKernel>().To<HitKernel>().ToSingleton();
            container.Bind<IColorKernel>().To<ColorKernel>().ToSingleton();
            container.Bind<ILocalSamplerProvider>().To<LocalSamplerProvider>().ToSingleton();
            container.Bind<ISceneConverter>().To<SceneConverter>().ToSingleton();
        }
    }
}
