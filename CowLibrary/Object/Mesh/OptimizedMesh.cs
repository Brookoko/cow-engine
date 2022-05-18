namespace CowLibrary
{
    using System.Numerics;

    public struct OptimizedMesh : IMesh
    {
        public KdTree tree;
        public readonly TriangleMesh mesh;

        public int Id { get; }
        
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

        public readonly Bound GetBoundingBox()
        {
            return mesh.GetBoundingBox();
        }
        
        public void Apply(in Matrix4x4 matrix)
        {
            mesh.Apply(in matrix);
            tree = KdTreeBuilder.Build(mesh.triangles, Id);
        }
    }
}
