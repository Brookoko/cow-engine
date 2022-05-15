namespace CowLibrary;

using System.Numerics;

public struct Surfel
{
    public RayHit hit;
    public Vector3 ray;
    public IMaterial material;
}
