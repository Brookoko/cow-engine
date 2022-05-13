namespace CowEngine
{
    using Cowject;
    using ImageWorker;

    public class CoreModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<IArgumentsParser>().To<ArgumentsParser>().ToSingleton();
            container.Bind<IIoWorker>().To<IoWorker>().ToSingleton();
            container.Bind<IWatch>().To<Watch>().ToSingleton();
            container.Bind<ISceneLoader>().To<SceneLoader>().ToSingleton();
            container.Bind<IFlow<CpuOption>>().To<CpuFlow>().ToSingleton();
            container.Bind<IFlow<GpuOption>>().To<GpuFlow>().ToSingleton();
        }
    }
}
