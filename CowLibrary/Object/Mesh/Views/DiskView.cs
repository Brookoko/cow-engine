namespace CowLibrary.Views;

using System.Numerics;

public readonly struct DiskView : IIntersectable
{
    public readonly Vector3 point;
    public readonly Vector3 normal;
    public readonly float radius;

    public int Id { get; }

    public DiskView(Vector3 point, Vector3 normal, float radius, int id)
    {
        this.point = point;
        this.normal = normal;
        this.radius = radius;
        Id = id;
    }

    public void Intersect(in Ray ray, ref RayHit best)
    {
        var dot = -Vector3.Dot(normal, ray.direction);
        if (dot <= Const.Epsilon)
        {
            return;
        }
        var dir = ray.origin - point;
        var t = Vector3.Dot(dir, normal) / dot;
        if (t <= 0 || t >= best.t)
        {
            return;
        }
        var p = ray.GetPoint(t);
        var dist = Vector3.DistanceSquared(p, point);
        if (dist > radius * radius)
        {
            return;
        }

        best = new RayHit(t, p, normal, Id);
    }
}
