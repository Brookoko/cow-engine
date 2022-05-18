namespace CowLibrary;

using System;
using System.Numerics;

public struct Bound : IIntersectable
{
    public Vector3 center;
    public Vector3 min;
    public Vector3 max;
    public Vector3 size;

    public Bound(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
        center = new Vector3((min.X + max.X) * 0.5f, (min.Y + max.Y) * 0.5f, (min.Z + max.Z) * 0.5f);
        size = max - center;
    }

    public Bound(Vector3 center, float sideLength)
    {
        this.center = center;
        size = new Vector3(sideLength / 2);
        min = center - size;
        max = center + size;
    }

    public readonly RayHit Intersect(in Ray ray)
    {
        var invdir = new Vector3(1 / ray.direction.X, 1 / ray.direction.Y, 1 / ray.direction.Z);

        var xmin = invdir.X >= 0 ? min.X : max.X;
        var xmax = invdir.X >= 0 ? max.X : min.X;
        var tmin = (xmin - ray.origin.X) * invdir.X;
        var tmax = (xmax - ray.origin.X) * invdir.X;

        var ymin = invdir.Y >= 0 ? min.Y : max.Y;
        var ymax = invdir.Y >= 0 ? max.Y : min.Y;
        var tymin = (ymin - ray.origin.Y) * invdir.Y;
        var tymax = (ymax - ray.origin.Y) * invdir.Y;

        if (tmin > tymax || tymin > tmax)
        {
            return Const.Miss;
        }
        tmin = Math.Max(tmin, tymin);
        tmax = Math.Min(tmax, tymax);

        var zmin = invdir.Z >= 0 ? min.Z : max.Z;
        var zmax = invdir.Z >= 0 ? max.Z : min.Z;
        var tzmin = (zmin - ray.origin.Z) * invdir.Z;
        var tzmax = (zmax - ray.origin.Z) * invdir.Z;

        if (tmin > tzmax || tzmin > tmax)
        {
            return Const.Miss;
        }
        tmin = Math.Max(tmin, tzmin);
        tmax = Math.Min(tmax, tzmax);

        float t;
        if (tmin < 0)
        {
            if (tmax < 0)
            {
                return Const.Miss;
            }
            t = tmax;
        }
        else
        {
            t = Math.Min(tmin, tmax);
        }

        var p = ray.GetPoint(t);

        return new RayHit(t, p, Vector3.Zero, -1);
    }
}
