namespace CowEngine.ImageWorker
{
    using Cowject;

    public class ImageModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<IPluginProvider>().To<PluginProvider>().ToSingleton();
            container.Bind<IImageEncoderProvider>().To<ImageEncoderProvider>().ToSingleton();
            container.Bind<IIoWorker>().To<IoWorker>().ToSingleton();
            container.Bind<IImageWorker>().To<ImageWorker>().ToSingleton();
        }
    }
}