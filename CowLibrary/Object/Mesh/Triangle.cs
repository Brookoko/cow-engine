namespace CowLibrary
{
    using System;
    using System.Numerics;

    public struct Triangle : IMesh
    {
        private Bound bound;

        private Vector3 v0;
        private Vector3 v1;
        private Vector3 v2;

        private Vector3 n0;
        private Vector3 n1;
        private Vector3 n2;

        public int Id { get; }

        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, int id) : this()
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            bound = CreateBound();
            Id = id;
        }

        private Bound CreateBound()
        {
            Vector3 min, max;
            min.X = Math.Min(v0.X, Math.Min(v1.X, v2.X));
            min.Y = Math.Min(v0.Y, Math.Min(v1.Y, v2.Y));
            min.Z = Math.Min(v0.Z, Math.Min(v1.Z, v2.Z));
            max.X = Math.Max(v0.X, Math.Max(v1.X, v2.X));
            max.Y = Math.Max(v0.Y, Math.Max(v1.Y, v2.Y));
            max.Z = Math.Max(v0.Z, Math.Max(v1.Z, v2.Z));
            return new Bound(min, max);
        }

        public void SetNormal(Vector3 n0, Vector3 n1, Vector3 n2)
        {
            this.n0 = n0.Normalize();
            this.n1 = n1.Normalize();
            this.n2 = n2.Normalize();
        }

        public void CalculateNormal()
        {
            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;
            var n = Vector3.Cross(v0v2, v0v1);
            n0 = n1 = n2 = n;
        }

        public readonly RayHit Intersect(in Ray ray)
        {
            var edge1 = v1 - v0;
            var edge2 = v2 - v0;

            var h = Vector3.Cross(ray.direction, edge2);
            var a = Vector3.Dot(edge1, h);
            if (Math.Abs(a) < Const.Epsilon)
            {
                return Const.Miss;
            }

            var f = 1f / a;
            var s = ray.origin - v0;
            var u = f * Vector3.Dot(s, h);
            if (u < 0 || u > 1)
            {
                return Const.Miss;
            }

            var q = Vector3.Cross(s, edge1);
            var v = f * Vector3.Dot(ray.direction, q);
            if (v < 0 || u + v > 1)
            {
                return Const.Miss;
            }

            var t = f * Vector3.Dot(edge2, q);
            if (t <= 0)
            {
                return Const.Miss;
            }

            var normal = n0 * (1 - u - v) + n1 * u + n2 * v;
            return new RayHit(t, ray.GetPoint(t), normal, Id);
        }

        public Bound GetBoundingBox()
        {
            return bound;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            v0 = matrix.MultiplyPoint(v0);
            v1 = matrix.MultiplyPoint(v1);
            v2 = matrix.MultiplyPoint(v2);
            n0 = matrix.MultiplyVector(n0).Normalize();
            n1 = matrix.MultiplyVector(n1).Normalize();
            n2 = matrix.MultiplyVector(n2).Normalize();
            bound = CreateBound();
        }
    }
}
