namespace ILGPURenderer;

using CowLibrary;

public struct LocalIntegrator
{
    public Color GetColor(in RayHit hit)
    {
        return hit.HasHit ? Color.White : Color.Black;
    }
}
