namespace CowLibrary
{
    using System;
    using System.Linq;
    using System.Numerics;

    public struct TriangleMesh : IMesh
    {
        public Bound BoundingBox { get; private set; }

        public readonly Triangle[] triangles;

        public TriangleMesh()
        {
            triangles = new Triangle[0];
            BoundingBox = new Bound();
        }
        
        public TriangleMesh(Triangle[] triangles) : this()
        {
            this.triangles = triangles;
            BoundingBox = IntersectionHelper.CreateBound(triangles);
        }

        public readonly RayHit? Intersect(in Ray ray)
        {
            RayHit? surfel = null;
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
            BoundingBox = IntersectionHelper.CreateBound(triangles);
        }
    }
}
