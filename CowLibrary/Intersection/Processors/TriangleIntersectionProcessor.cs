namespace CowLibrary.Processors
{
    using System;
    using System.Numerics;

    public static class TriangleIntersectionProcessor
    {
        private const double Tollerance = 0.00000001d;
        
        public static bool CheckForIntersection(Triangle triangle, Ray ray, out Surfel intersectionSurfel)
        {
            if (FindIntersectionPoint(triangle, ray, out var intersectionPoint))
            {
                intersectionSurfel = new Surfel()
                {
                    point = intersectionPoint,
                    normal = (triangle.n0 + triangle.n1 + triangle.n2)/3
                };
                return true;
            }

            intersectionSurfel = null;
            return false;
        }

        // https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
        // Möller–Trumbore intersection algorithm
        private static bool FindIntersectionPoint(Triangle triangle, Ray ray, out Vector3 intersectionPoint)
        {
            var vertex0 = triangle.v0;
            var vertex1 = triangle.v1;
            var vertex2 = triangle.v2;
            Vector3 h, s, q;
            double a, f, u, v;

            var edge1 = vertex1 - vertex0;
            var edge2 = vertex2 - vertex0;

            h = Vector3.Cross(ray.direction, edge2);
            a = Vector3.Dot(edge1, h);
            if (Math.Abs(a) < Tollerance)
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            f = 1f / a;
            s = ray.origin - vertex0;
            u = f * Vector3.Dot(s, h);
            if (u < 0 || u > 1)
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(ray.direction, q);
            if (v < 0 || u + v > 1)
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            var t = f * Vector3.Dot(edge2, q);
            if (t > Tollerance)
            {
                intersectionPoint = ray.origin + ray.direction * (float) t;
                return true;
            }

            intersectionPoint = Vector3.Zero;
            return false;
        }
    }
}