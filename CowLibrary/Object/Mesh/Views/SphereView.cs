﻿namespace CowLibrary.Object.Mesh.Views;

using System;
using System.Numerics;

public readonly struct SphereView : IIntersectable
{
    public readonly Vector3 center;
    public readonly float radius;

    public int Id { get; }

    public SphereView(Vector3 center, float radius, int id)
    {
        this.center = center;
        this.radius = radius;
        Id = id;
    }

    public void Intersect(in Ray ray, ref RayHit best)
    {
        var f1 = ray.origin.X - center.X;
        var f2 = ray.origin.Y - center.Y;
        var f3 = ray.origin.Z - center.Z;
        var aCoeff = ray.direction.X * ray.direction.X +
                     ray.direction.Y * ray.direction.Y +
                     ray.direction.Z * ray.direction.Z;
        var halfBCoeff = ray.direction.X * f1 + ray.direction.Y * f2 + ray.direction.Z * f3;
        var cCoeff = f1 * f1 + f2 * f2 + f3 * f3 - radius * radius;

        var discriminant = halfBCoeff * halfBCoeff - aCoeff * cCoeff;
        if (discriminant < 0)
        {
            return;
        }

        float t;
        int sign = 1;
        if (discriminant == 0)
        {
            t = (float)Math.Sqrt(aCoeff * cCoeff);
        }
        else
        {
            var sqrDiscriminant = Math.Sqrt(discriminant);
            var k1 = (-halfBCoeff + sqrDiscriminant) / aCoeff;
            var k2 = (-halfBCoeff - sqrDiscriminant) / aCoeff;
            if (k1 < 0 || k2 < 0)
            {
                sign = -1;
            }

            k1 = k1 > 0 ? k1 : k2;
            k2 = k2 > 0 ? k2 : k1;
            if (k2 < 0)
            {
                return;
            }

            t = (float)Math.Min(k1, k2);
        }
        if (t >= best.t)
        {
            return;
        }

        var p = ray.GetPoint(t);
        best = new RayHit(t, p, sign * (p - center).Normalize(), Id);
    }
}
