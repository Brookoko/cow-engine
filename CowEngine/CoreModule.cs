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
            container.Bind<IFlow>().WithName(Options.Model).To<ModelFlow>().ToSingleton();
            container.Bind<IFlow>().WithName(Options.Compiled).To<CompiledFlow>().ToSingleton();
            container.Bind<IFlow>().WithName(Options.Scene).To<SceneFlow>().ToSingleton();
        }
    }
}
