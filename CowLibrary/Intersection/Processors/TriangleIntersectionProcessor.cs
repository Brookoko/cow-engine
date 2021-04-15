namespace CowLibrary.Processors
{
    using System;
    using System.Numerics;

    public static class TriangleIntersectionProcessor
    {
        private const double Tolerance = 0.00001d;
        
        public static bool CheckForIntersection(Triangle triangle, Ray ray, out Surfel intersectionSurfel)
        {
            if (FindIntersectionPoint(triangle, ray, out var intersectionDistance))
            {
                intersectionSurfel = new Surfel()
                {
                    point = ray.origin + ray.direction * intersectionDistance,
                    normal = triangle.n0.Normalize(),
                    t = intersectionDistance
                };
                return true;
            }

            intersectionSurfel = null;
            return false;
        }

        // https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
        // Möller–Trumbore intersection algorithm
        private static bool FindIntersectionPoint(Triangle triangle, Ray ray, out float distance)
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
            if (Math.Abs(a) < Tolerance)
            {
                distance = 0;
                return false;
            }

            f = 1f / a;
            s = ray.origin - vertex0;
            u = f * Vector3.Dot(s, h);
            if (u < 0 || u > 1)
            {
                distance = 0;
                return false;
            }

            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(ray.direction, q);
            if (v < 0 || u + v > 1)
            {
                distance = 0;
                return false;
            }

            var t = f * Vector3.Dot(edge2, q);
            if (t > Tolerance)
            {
                distance = (float) t;
                return true;
            }

            distance = 0;
            return false;
        }
    }
}