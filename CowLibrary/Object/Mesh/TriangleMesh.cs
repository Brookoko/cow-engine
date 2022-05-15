namespace CowLibrary
{
    using System.Numerics;

    public struct TriangleMesh : IMesh
    {
        public readonly Triangle[] triangles;
        private Bound bound;

        public TriangleMesh()
        {
            triangles = new Triangle[0];
            bound = new Bound();
        }

        public TriangleMesh(Triangle[] triangles) : this()
        {
            this.triangles = triangles;
            bound = IntersectionHelper.CreateBound(triangles);
        }

        public readonly RayHit Intersect(in Ray ray)
        {
            var hit = new RayHit();
            for (var i = 0; i < triangles.Length; i++)
            {
                var tHit = triangles[i].Intersect(in ray);
                if (hit.t > tHit.t)
                {
                    hit = tHit;
                }
            }
            return hit;
        }

        public Bound GetBoundingBox()
        {
            return bound;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            foreach (var triangle in triangles)
            {
                triangle.Apply(in matrix);
            }
            bound = IntersectionHelper.CreateBound(triangles);
        }
    }
}
