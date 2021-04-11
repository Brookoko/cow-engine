namespace CowLibrary.Processors
{
    using System;
    using System.Numerics;

    public class SphereIntersectionProcessor
    {
        public static bool CheckForIntersection(Sphere sphere, Ray ray, out Surfel intersectionSurfel)
        {
            if (FindIntersectionPoint(sphere, ray, out var intersectionPoint))
            {
                intersectionSurfel = new Surfel()
                {
                    point = intersectionPoint,
                    normal = intersectionPoint - sphere.center
                };
            }

            intersectionSurfel = null;
            return false;
        }

        private static bool FindIntersectionPoint(Sphere sphere, Ray ray, out Vector3 intersectionPoint)
        {
            // sphere (center (x0,y0,z0), radius R): (x - x0)^2 + (y - y0)^2 + (z-z0)^2 = R^2
            // ray (origin (x0,y0,z0), direction (x1,y1,z1)) : x = x0 + x1 * k; y = y0 + y1*k; z = z0 + z1*k

            var f1 = ray.origin.X - sphere.center.X;
            var f2 = ray.origin.Y - sphere.center.Y;
            var f3 = ray.origin.Z - sphere.center.Z;
            var aCoeff = ray.direction.X * ray.direction.X +
                         ray.direction.Y * ray.direction.Y +
                         ray.direction.Z * ray.direction.Z;
            var halfBCoeff = ray.direction.X * f1 + ray.direction.X * f2 + ray.direction.Z * f3;
            var cCoeff = f1 * f1 + f2 * f2 + f3 * f3 + sphere.radius * sphere.radius;

            var discriminant = Math.Sqrt(halfBCoeff * halfBCoeff - aCoeff * cCoeff);
            if (discriminant < 0)
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            if (discriminant == 0)
            {
                var k = (float) Math.Sqrt(aCoeff * cCoeff);
                intersectionPoint = ray.origin + k * ray.direction;
                return true;
            }

            var sqrDiscriminant = Math.Sqrt(discriminant);
            var k1 = (-halfBCoeff + sqrDiscriminant) / aCoeff;
            var k2 = (-halfBCoeff - sqrDiscriminant) / aCoeff;
            ;
            k1 = k1 > 0 ? k1 : k2;
            k2 = k2 > 0 ? k2 : k1;
            if (k2 < 0)
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            intersectionPoint = ray.origin + (float) Math.Min(k1, k2) * ray.direction;
            return true;
        }
    }
}