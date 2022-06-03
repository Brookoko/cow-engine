namespace CowEngine;

using System;
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
        var total = 0.0;
        Watch.Start();
        var scene = SceneLoader.LoadSceneFromOptions(option);
        total += Watch.Stop("Loading scene");

        Watch.Start();
        scene.PrepareScene();
        total += Watch.Stop("Preparing scene");

        Watch.Start();
        var renderer = RendererProvider.GetRenderer(scene, option.Mode);
        total += Watch.Stop("Preparing renderer");

        Watch.Start();
        var image = renderer.Render(scene);
        total += Watch.Stop("Rendering scene");

        Watch.Start();
        ImageWorker.SaveImage(in image, option.Output);
        total += Watch.Stop("Saving render");

        Console.WriteLine($"Total time: {total}");
        return 0;
    }
}
