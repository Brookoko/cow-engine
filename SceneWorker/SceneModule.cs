namespace CowEngine
{
    using Cowject;
    using SceneFormat;
    using SceneWorker;

    public class SceneModule : IModule
    {
        public void Prepare(DiContainer container)
        {
            container.Bind<ISceneIO>().To<SceneIO>().ToSingleton();
            container.Bind<ISceneWorker>().To<SceneWorker>().ToSingleton();
        }
    }
}
