namespace CowLibrary
{
    using System.Numerics;

    public struct TriangleMesh : IMesh
    {
        public int Id { get; }

        public readonly Bound BoundingBox => bound;

        public readonly Triangle[] triangles;
        private Bound bound;

        public TriangleMesh()
        {
            triangles = new Triangle[0];
            bound = new Bound();
            Id = -1;
        }

        public TriangleMesh(Triangle[] triangles, int id) : this()
        {
            this.triangles = triangles;
            bound = IntersectionHelper.CreateBound(triangles, id);
            Id = id;
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            for (var i = 0; i < triangles.Length; i++)
            {
                triangles[i].Intersect(in ray, ref best);
            }
        }

        public void Apply(in Matrix4x4 matrix)
        {
            foreach (var triangle in triangles)
            {
                triangle.Apply(in matrix);
            }
            bound = IntersectionHelper.CreateBound(triangles, Id);
        }
    }
}
