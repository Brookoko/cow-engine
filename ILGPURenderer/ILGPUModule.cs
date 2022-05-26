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
            container.Bind<IRenderKernel>().To<RenderKernel>().ToSingleton();
            container.Bind<ILocalSamplerProvider>().To<LocalSamplerProvider>().ToSingleton();
            container.Bind<ISceneConverter>().To<SceneConverter>().ToSingleton();
            
            container.BindInterfacesTo<ILGPURenderer>().ToSingleton();
        }
    }
}
