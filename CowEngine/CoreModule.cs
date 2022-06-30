namespace CowEngine
{
    using Cowject;
    using ImageWorker;

    public class CoreModule : IModule
    {
        public Priority Priority => Priority.Highest;

        public void Prepare(DiContainer container)
        {
            container.Bind<IBytesReader>().To<SimpleBytesReader>().ToSingleton();
            container.Bind<IBytesWriter>().ToInstance(new FilesBypassingWriter("_"));
            container.Bind<IIoWorker>().To<IoWorker>().ToSingleton();
            
            container.BindInterfacesTo<ArgumentsParser>().ToSingleton();
            container.BindInterfacesTo<Watch>().ToSingleton();
            container.BindInterfacesTo<SceneLoader>().ToSingleton();
            container.BindInterfacesTo<BasicFlow>().ToSingleton();
            container.BindInterfacesTo<RendererProvider>().ToSingleton();
        }
    }
}
