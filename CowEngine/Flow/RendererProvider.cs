namespace CowEngine;

using System;
using System.Linq;
using Cowject;
using CowRenderer;

public interface IRendererProvider
{
    IRenderer GetRenderer(string tag);
}

public class RendererProvider : IRendererProvider
{
    [Inject]
    public IRenderer[] Renderers { get; set; }

    public IRenderer GetRenderer(string tag)
    {
        var uniformTag = Uniform(tag);
        var renderer = Renderers.FirstOrDefault(r => r.Tag == uniformTag);
        if (renderer == null)
        {
            throw new Exception($"No renderer with tag: {tag}");
        }
        return renderer;
    }

    private string Uniform(string tag)
    {
        return tag.Trim().ToLowerInvariant();
    }
}
