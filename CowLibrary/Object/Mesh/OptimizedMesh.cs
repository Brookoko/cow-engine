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

        public readonly RayHit Intersect(in Ray ray)
        {
            return tree.Intersect(in ray);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            mesh.Apply(in matrix);
            tree = KdTreeBuilder.Build(mesh.triangles, Id);
        }
    }
}
