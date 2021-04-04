namespace CowEngine
{
    using Cowject;
    using ObjLoader.Loader.Loaders;

    public class ObjModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<IObjWorker>().To<ObjWorker>().ToSingleton();
            container.Bind<IObjLoaderFactory>().To<ObjLoaderFactory>().ToSingleton();
            container.Bind<IModelToObjectConverter>().To<ModelToObjectConverter>().ToSingleton();
        }
    }
}