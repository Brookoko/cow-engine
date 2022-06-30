namespace CowLibrary
{
    using System;
    using System.Numerics;
    using Views;

    public struct Triangle : IMesh<TriangleView>
    {
        public readonly int Id => view.Id;

        public readonly TriangleView View => view;

        public readonly Bound BoundingBox => bound;
        
        private TriangleView view;
        private Bound bound;

        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 n0, Vector3 n1, Vector3 n2, int id) : this()
        {
            view = new TriangleView(v0, v1, v2, n0, n1, n2, id);
            bound = CreateBound();
        }

        private Bound CreateBound()
        {
            Vector3 min, max;
            min.X = Math.Min(view.v0.X, Math.Min(view.v1.X, view.v2.X));
            min.Y = Math.Min(view.v0.Y, Math.Min(view.v1.Y, view.v2.Y));
            min.Z = Math.Min(view.v0.Z, Math.Min(view.v1.Z, view.v2.Z));
            max.X = Math.Max(view.v0.X, Math.Max(view.v1.X, view.v2.X));
            max.Y = Math.Max(view.v0.Y, Math.Max(view.v1.Y, view.v2.Y));
            max.Z = Math.Max(view.v0.Z, Math.Max(view.v1.Z, view.v2.Z));
            return new Bound(min, max, Id);
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            view.Intersect(in ray, ref best);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            var v0 = matrix.MultiplyPoint(view.v0);
            var v1 = matrix.MultiplyPoint(view.v1);
            var v2 = matrix.MultiplyPoint(view.v2);
            var n0 = matrix.MultiplyVector(view.n0).Normalize();
            var n1 = matrix.MultiplyVector(view.n1).Normalize();
            var n2 = matrix.MultiplyVector(view.n2).Normalize();
            view = new TriangleView(v0, v1, v2, n0, n1, n2, Id);
            bound = CreateBound();
        }
    }
}
