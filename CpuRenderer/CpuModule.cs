namespace CpuRenderer;

using Cowject;
using CowRenderer;
using CowRenderer.Integration;
using CowRenderer.Raycasting;
using CowRenderer.Rendering;

public class CpuModule : IModule
{
    public void Prepare(DiContainer container)
    {
        container.Bind<ThreadRenderer>().To<MultiRayThreadRenderer>();
        container.Bind<IRaycaster>().To<SimpleRaycaster>().ToSingleton();
        container.Bind<IIntegrator>().To<MaterialIntegrator>().ToSingleton();

        container.BindInterfacesTo<SimpleRenderer>().ToSingleton();
        container.BindInterfacesTo<MultithreadRenderer>().ToSingleton();
    }
}
