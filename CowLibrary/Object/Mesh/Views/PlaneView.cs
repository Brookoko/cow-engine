﻿namespace CowLibrary.Object.Mesh.Views;

using System.Numerics;

public readonly struct PlaneView : IIntersectable
{
    public readonly Vector3 point;
    public readonly Vector3 normal;

    public int Id { get; }

    public PlaneView(Vector3 point, Vector3 normal, int id)
    {
        this.point = point;
        this.normal = normal;
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
        best = new RayHit(t, ray.GetPoint(t), normal, Id);
    }
}
