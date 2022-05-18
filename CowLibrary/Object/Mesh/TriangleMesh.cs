namespace CowLibrary
{
    using System.Numerics;

    public struct TriangleMesh : IMesh
    {
        public readonly Triangle[] triangles;
        private Bound bound;

        public int Id { get; }

        public TriangleMesh()
        {
            triangles = new Triangle[0];
            bound = new Bound();
            Id = -1;
        }

        public TriangleMesh(Triangle[] triangles, int id) : this()
        {
            this.triangles = triangles;
            bound = IntersectionHelper.CreateBound(triangles);
            Id = id;
        }

        public readonly RayHit Intersect(in Ray ray)
        {
            var hit = Const.Miss;
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
