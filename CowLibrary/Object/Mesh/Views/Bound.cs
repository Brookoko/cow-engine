namespace CowLibrary;

using System;
using System.Numerics;

public readonly struct Bound : IIntersectable
{
    public int Id { get; }

    public readonly Vector3 center;
    public readonly Vector3 min;
    public readonly Vector3 max;
    public readonly Vector3 size;

    public Bound(Vector3 min, Vector3 max, int id)
    {
        this.min = min;
        this.max = max;
        center = new Vector3((min.X + max.X) * 0.5f, (min.Y + max.Y) * 0.5f, (min.Z + max.Z) * 0.5f);
        size = max - center;
        Id = id;
    }

    public Bound(Vector3 center, float sideLength, int id)
    {
        this.center = center;
        size = new Vector3(sideLength / 2);
        min = center - size;
        max = center + size;
        Id = id;
    }

    public void Intersect(in Ray ray, ref RayHit best)
    {
        var xmin = ray.invDirection.X >= 0 ? min.X : max.X;
        var xmax = ray.invDirection.X >= 0 ? max.X : min.X;
        var tmin = (xmin - ray.origin.X) * ray.invDirection.X;
        var tmax = (xmax - ray.origin.X) * ray.invDirection.X;

        var ymin = ray.invDirection.Y >= 0 ? min.Y : max.Y;
        var ymax = ray.invDirection.Y >= 0 ? max.Y : min.Y;
        var tymin = (ymin - ray.origin.Y) * ray.invDirection.Y;
        var tymax = (ymax - ray.origin.Y) * ray.invDirection.Y;

        if (tmin > tymax || tymin > tmax)
        {
            return;
        }
        tmin = Math.Max(tmin, tymin);
        tmax = Math.Min(tmax, tymax);

        var zmin = ray.invDirection.Z >= 0 ? min.Z : max.Z;
        var zmax = ray.invDirection.Z >= 0 ? max.Z : min.Z;
        var tzmin = (zmin - ray.origin.Z) * ray.invDirection.Z;
        var tzmax = (zmax - ray.origin.Z) * ray.invDirection.Z;

        if (tmin > tzmax || tzmin > tmax)
        {
            return;
        }
        tmin = Math.Max(tmin, tzmin);
        tmax = Math.Min(tmax, tzmax);

        var t = tmin;
        t = t < 0 ? tmax : t;
        if (t < 0 || t >= best.t)
        {
            return;
        }

        best = new RayHit(t, ray.GetPoint(t), Vector3.Zero, Id);
    }
}
