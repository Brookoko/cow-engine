namespace CowLibrary.Views;

using System;
using System.Numerics;

public readonly struct TriangleView : IIntersectable
{
    public readonly Vector3 v0;
    public readonly Vector3 v1;
    public readonly Vector3 v2;

    public readonly Vector3 n0;
    public readonly Vector3 n1;
    public readonly Vector3 n2;

    public int Id { get; }

    public TriangleView(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 n0, Vector3 n1, Vector3 n2, int id) : this()
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
        this.n0 = n0;
        this.n1 = n1;
        this.n2 = n2;
        Id = id;
    }

    public void Intersect(in Ray ray, ref RayHit best)
    {
        var edge1 = v1 - v0;
        var edge2 = v2 - v0;

        var h = Vector3.Cross(ray.direction, edge2);
        var a = Vector3.Dot(edge1, h);
        if (Math.Abs(a) < Const.Epsilon)
        {
            return;
        }

        var f = 1f / a;
        var s = ray.origin - v0;
        var u = f * Vector3.Dot(s, h);
        if (u < 0 || u > 1)
        {
            return;
        }

        var q = Vector3.Cross(s, edge1);
        var v = f * Vector3.Dot(ray.direction, q);
        if (v < 0 || u + v > 1)
        {
            return;
        }

        var t = f * Vector3.Dot(edge2, q);
        if (t <= 0 || t >= best.t)
        {
            return;
        }

        var normal = n0 * (1 - u - v) + n1 * u + n2 * v;
        best = new RayHit(t, ray.GetPoint(t), normal, Id);
    }
}
