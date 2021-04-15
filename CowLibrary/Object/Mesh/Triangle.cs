namespace CowLibrary
{
    using System;
    using System.Numerics;
    using Processors;

    public class Triangle : Mesh
    {
        public Vector3 v0;
        public Vector3 v1;
        public Vector3 v2;
        
        public Vector3 n0;
        public Vector3 n1;
        public Vector3 n2;
        
        public override Box BoundingBox { get; }
        
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            
            Vector3 min, max;
            min.X = Math.Min(v0.X, Math.Min(v1.X, v2.X));
            min.Y = Math.Min(v0.Y, Math.Min(v1.Y, v2.Y));
            min.Z = Math.Min(v0.Z, Math.Min(v1.Z, v2.Z));
            max.X = Math.Max(v0.X, Math.Max(v1.X, v2.X));
            max.Y = Math.Max(v0.Y, Math.Max(v1.Y, v2.Y));
            max.Z = Math.Max(v0.Z, Math.Max(v1.Z, v2.Z));
            BoundingBox = new Box(min, max);
        }
        
        public void SetNormal(Vector3 n0, Vector3 n1, Vector3 n2)
        {
            this.n0 = n0;
            this.n1 = n1;
            this.n2 = n2;
        }
        
        public void CalculateNormal()
        {
            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;
            var n = Vector3.Cross(v0v2, v0v1);
            n0 = n1 = n2 = n;
        }

        public override bool Intersect(Ray ray, out Surfel surfel)
            => TriangleIntersectionProcessor.CheckForIntersection(this, ray, out surfel);
        
        public override void Apply(Matrix4x4 matrix)
        {
            v0 = matrix.MultiplyPoint(v0);
            v1 = matrix.MultiplyPoint(v1);
            v2 = matrix.MultiplyPoint(v2);
            if (Matrix4x4.Invert(matrix, out var m))
            {
                m = Matrix4x4.Transpose(m);
                n0 = m.MultiplyVector(n0);
                n1 = m.MultiplyVector(n1);
                n2 = m.MultiplyVector(n2);
            }
        }
    }
}