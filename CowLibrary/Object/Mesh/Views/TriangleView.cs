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
        if (!GetDerivatives(out var dpdu, out var dpdv))
        {
            return;
        }

        var normal = n0 * (1 - u - v) + n1 * u + n2 * v;
        best = new RayHit(t, ray.GetPoint(t), normal, dpdu, Id);
    }

    private bool GetDerivatives(out Vector3 dpdu, out Vector3 dpdv)
    {
        dpdu = Vector3.Zero;
        dpdv = Vector3.Zero;

        var uv0 = new Vector2(0, 0);
        var uv1 = new Vector2(1, 0);
        var uv2 = new Vector2(1, 1);

        var duv02 = uv0 - uv2;
        var duv12 = uv1 - uv2;
        var dp02 = v0 - v2;
        var dp12 = v1 - v2;

        var determinant = duv02.X * duv12.Y - duv02.Y * duv12.X;
        var degenerateUv = Math.Abs(determinant) < 1e-8;
        if (!degenerateUv)
        {
            var invdet = 1 / determinant;
            dpdu = (duv12.Y * dp02 - duv02.Y * dp12) * invdet;
            dpdv = (-duv12.X * dp02 + duv02.X * dp12) * invdet;
        }
        if (degenerateUv || Vector3.Cross(dpdu, dpdv).LengthSquared() == 0)
        {
            var ng = Vector3.Cross(v2 - v0, v1 - v0);
            if (ng.LengthSquared() == 0)
            {
                return false;
            }
            var n = ng.Normalize();
            Mathf.CoordinateSystem(ref n, ref dpdu, ref dpdv);
        }
        return true;
    }
}
