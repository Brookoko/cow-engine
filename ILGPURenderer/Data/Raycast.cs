namespace ILGPURenderer.Data;

using CowLibrary;

public readonly struct Raycast
{
    public readonly RayHit hit;
    public readonly Ray ray;

    public Raycast(RayHit hit, Ray ray)
    {
        this.hit = hit;
        this.ray = ray;
    }
}
