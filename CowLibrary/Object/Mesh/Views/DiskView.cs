namespace CowLibrary.Views;

using System;
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
        var hit = ray.GetPoint(t);
        var p = hit - point;
        var dist = Vector3.Dot(p, p);
        if (dist > radius * radius)
        {
            return;
        }

        var rHit = (float)Math.Sqrt(dist);
        var dpdu = new Vector3(-Const.PhiMax * p.Z, 0, Const.PhiMax * p.X);
        var dpdv = new Vector3(p.X, 0, p.Z) * -radius / rHit;

        best = new RayHit(t, hit, normal, dpdu, Id);
    }
}
