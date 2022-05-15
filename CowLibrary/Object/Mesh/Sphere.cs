namespace CowLibrary
{
    using System;
    using System.Numerics;

    public struct Sphere : IMesh
    {
        public Bound BoundingBox { get; private set; }

        private float radius;
        private Vector3 center;

        public Sphere(float radius) : this()
        {
            this.radius = radius;
            center = Vector3.Zero;
            BoundingBox = CreateBound();
        }

        private Bound CreateBound()
        {
            return new Bound(center, radius * 2);
        }

        public readonly RayHit? Intersect(in Ray ray)
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
                return null;
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
                    return null;
                }

                t = (float)Math.Min(k1, k2);
            }

            var p = ray.GetPoint(t);
            return new RayHit()
            {
                point = p,
                normal = (p - center).Normalize(),
                t = t
            };
        }

        public void Apply(in Matrix4x4 matrix)
        {
            center = matrix.MultiplyPoint(center);
            radius = matrix.ExtractScale().Min() * radius;
            BoundingBox = CreateBound();
        }
    }
}
