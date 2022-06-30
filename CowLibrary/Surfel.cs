namespace CowLibrary;

using System.Numerics;

public readonly struct Surfel
{
    public readonly RayHit hit;
    public readonly Vector3 ray;
    public readonly IMaterial material;

    public Surfel(Vector3 ray)
    {
        hit = Const.Miss;
        this.ray = ray;
        material = default;
    }

    public Surfel(RayHit hit, Vector3 ray, IMaterial material)
    {
        this.hit = hit;
        this.ray = ray;
        this.material = material;
    }
}
