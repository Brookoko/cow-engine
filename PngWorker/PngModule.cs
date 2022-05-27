namespace CowEngine.ImageWorker.Png;

using Cowject;

public class PngModule : IModule
{
    public Priority Priority => Priority.High;

    public void Prepare(DiContainer container)
    {
        container.Bind<Deflate>().To<Deflate>().ToSingleton();
        container.BindInterfacesTo<ImageByteConverter>().ToSingleton();
        container.BindInterfacesTo<ByteFilterer>().ToSingleton();
        container.BindInterfacesTo<PngWorker>().ToSingleton();
    }
}
