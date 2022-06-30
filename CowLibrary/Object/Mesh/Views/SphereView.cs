namespace CowLibrary.Object.Mesh.Views;

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
        if (discriminant == 0)
        {
            t = (float)Math.Sqrt(aCoeff * cCoeff);
        }
        else
        {
            var sqrDiscriminant = Math.Sqrt(discriminant);
            var k1 = (-halfBCoeff + sqrDiscriminant) / aCoeff;
            var k2 = (-halfBCoeff - sqrDiscriminant) / aCoeff;

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
        var normal = (p - center).Normalize();
        var (dpdu, dpdv) = GetDerivatives(p);
        best = new RayHit(t, p, normal, dpdu, Id);
    }

    private (Vector3 dpdu, Vector3 dpdv) GetDerivatives(Vector3 point)
    {
        var p = (point - center).Normalize();
        var theta = (float)Math.Acos(Mathf.Clamp(p.Y / radius, -1, 1));
        var thetaMin = -1 / radius;
        var thetaMax = 1 / radius;

        var zRadius = (float)Math.Sqrt(p.X * p.X + p.Z * p.Z);
        var invZRadius = 1 / zRadius;
        var cosPhi = p.X * invZRadius;
        var sinPhi = p.Z * invZRadius;

        var dpdu = new Vector3(-Const.PhiMax * p.Z, 0, Const.PhiMax * p.X);
        var dpdv = (thetaMax - thetaMin) * new Vector3(p.Y * cosPhi, -radius * (float)Math.Sin(theta), p.Y * sinPhi);
        return (dpdu, dpdv);
    }
}
