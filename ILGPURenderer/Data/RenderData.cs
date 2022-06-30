namespace ILGPURenderer.Data;

using CowRenderer;

public readonly struct RenderData
{
    public readonly int numberOfRayPerPixelDimension;
    public readonly int rayDepth;

    public RenderData(RenderConfig renderConfig)
    {
        numberOfRayPerPixelDimension = renderConfig.numberOfRayPerPixelDimension;
        rayDepth = renderConfig.rayDepth;
    }
}
