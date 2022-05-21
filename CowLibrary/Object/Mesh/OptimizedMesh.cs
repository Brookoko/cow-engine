namespace CowLibrary
{
    using System.Numerics;

    public struct OptimizedMesh : IMesh
    {
        public readonly TriangleMesh mesh;
        public KdTree tree;

        public int Id { get; }

        public readonly Bound BoundingBox => mesh.BoundingBox;

        public OptimizedMesh(Triangle[] triangles, int id)
        {
            mesh = new TriangleMesh(triangles, id);
            tree = default;
            Id = id;
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            tree.Intersect(in ray, ref best);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            mesh.Apply(in matrix);
            tree = KdTreeBuilder.Build(mesh.triangles, Id);
        }
    }
}
