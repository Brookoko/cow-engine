namespace ILGPURenderer.Data;

using CowRenderer;

public readonly struct RenderData
{
    public readonly int numberOfRayPerPixelDimension;
    public readonly int rayDepth;
    public readonly int numberOfRayPerMaterial;

    public RenderData(RenderConfig renderConfig)
    {
        numberOfRayPerPixelDimension = renderConfig.numberOfRayPerPixelDimension;
        rayDepth = renderConfig.rayDepth;
        numberOfRayPerMaterial = renderConfig.numberOfRayPerMaterial;
    }
}
