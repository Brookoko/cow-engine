namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class TriangleMesh : Mesh
    {
        public readonly List<Triangle> triangles;
        
        public override Box BoundingBox => box;
        
        private Box box;
        
        public TriangleMesh(List<Triangle> triangles)
        {
            this.triangles = triangles;
            box = CreateBox();
        }
        
        private Box CreateBox()
        {
            var min = Vector3.One * float.MaxValue;
            var max = Vector3.One * float.MinValue;
            foreach (var box in triangles.Select(t => t.BoundingBox))
            {
                min.X = Math.Min(min.X, box.min.X);
                min.Y = Math.Min(min.Y, box.min.Y);
                min.Z = Math.Min(min.Z, box.min.Z);
                max.X = Math.Max(max.X, box.max.X);
                max.Y = Math.Max(max.Y, box.max.Y);
                max.Z = Math.Max(max.Z, box.max.Z);
            }
            return new Box(min, max);
        }
        
        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            surfel = null;
            var intersected = false;
            foreach (var t in triangles)
            {
                if (t.Intersect(ray, out var s))
                {
                    intersected = true;
                    if (surfel == null || surfel.t > s.t)
                    {
                        surfel = s;
                    }
                }
            }
            return intersected;
        }
        
        public override void Apply(Matrix4x4 matrix)
        {
            foreach (var triangle in triangles)
            {
                triangle.Apply(matrix);
            }
            box = CreateBox();
        }
    }
}