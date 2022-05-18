namespace ILGPURenderer.Data;

using CowRenderer;

public readonly struct RenderData
{
    public readonly int numberOfRayPerPixel;
    public readonly int rayDepth;
    public readonly int numberOfRayPerMaterial;

    public RenderData(RenderConfig renderConfig)
    {
        numberOfRayPerPixel = renderConfig.numberOfRayPerPixel;
        rayDepth = renderConfig.rayDepth;
        numberOfRayPerMaterial = renderConfig.numberOfRayPerMaterial;
    }
}
