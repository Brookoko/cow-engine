namespace CowEngine;

using System;
using System.Linq;
using Cowject;
using CowRenderer;

public interface IRendererProvider
{
    IRenderer GetRenderer(Scene scene, string tag);
}

public class RendererProvider : IRendererProvider
{
    [Inject]
    public IRenderer[] Renderers { get; set; }

    public IRenderer GetRenderer(Scene scene, string tag)
    {
        var uniformTag = Uniform(tag);
        var renderer = Renderers.FirstOrDefault(r => r.Tag == uniformTag);
        if (renderer == null)
        {
            throw new Exception($"No renderer with tag: {tag}");
        }
        renderer.Prepare(scene);
        return renderer;
    }

    private string Uniform(string tag)
    {
        return tag.Trim().ToLowerInvariant();
    }
}
