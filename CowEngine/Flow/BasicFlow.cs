namespace CowEngine;

using Cowject;
using CowLibrary;
using ImageWorker;

public class BasicFlow : IFlow
{
    [Inject]
    public IRendererProvider RendererProvider { get; set; }

    [Inject]
    public IWatch Watch { get; set; }

    [Inject]
    public ISceneLoader SceneLoader { get; set; }

    [Inject]
    public IImageWorker ImageWorker { get; set; }

    public bool CanWorkWithProcess(Option option)
    {
        return true;
    }

    public int Process(Option option)
    {
        Watch.Start();
        var scene = SceneLoader.LoadSceneFromOptions(option);
        Watch.Stop("Loading scene");

        Watch.Start();
        scene.PrepareScene();
        Watch.Stop("Preparing scene");

        Watch.Start();
        var renderer = RendererProvider.GetRenderer(option.Mode);
        var image = renderer.Render(scene);
        Watch.Stop("Rendering scene");

        Watch.Start();
        ImageWorker.SaveImage(in image, option.Output);
        Watch.Stop("Saving render");

        return 0;
    }
}
