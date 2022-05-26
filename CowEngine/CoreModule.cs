namespace CowEngine
{
    using Cowject;
    using ImageWorker;

    public class CoreModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.BindInterfacesTo<ArgumentsParser>().ToSingleton();
            container.BindInterfacesTo<IoWorker>().ToSingleton();
            container.BindInterfacesTo<Watch>().ToSingleton();
            container.BindInterfacesTo<SceneLoader>().ToSingleton();
            container.BindInterfacesTo<BasicFlow>().ToSingleton();
            container.BindInterfacesTo<RendererProvider>().ToSingleton();
        }
    }
}
