namespace CowLibrary
{
    using System;
    using System.Linq;
    using System.Numerics;

    public struct TriangleMesh : IMesh
    {
        public Box BoundingBox { get; private set; }

        public readonly Triangle[] triangles;

        public TriangleMesh(Triangle[] triangles) : this()
        {
            this.triangles = triangles;
            BoundingBox = CreateBox();
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

        public readonly Surfel? Intersect(in Ray ray)
        {
            Surfel? surfel = null;
            foreach (var t in triangles)
            {
                var s = t.Intersect(in ray);
                if (s.HasValue)
                {
                    if (!surfel.HasValue || surfel.Value.t > s.Value.t)
                    {
                        surfel = s;
                    }
                }
            }
            return surfel;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            foreach (var triangle in triangles)
            {
                triangle.Apply(in matrix);
            }
            BoundingBox = CreateBox();
        }
    }
}
