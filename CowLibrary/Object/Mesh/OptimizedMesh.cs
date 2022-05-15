namespace CowLibrary
{
    using System.Numerics;

    public struct OptimizedMesh : IMesh
    {
        public Bound BoundingBox => mesh.BoundingBox;

        private readonly TriangleMesh mesh;
        private KdTree tree;

        public OptimizedMesh(Triangle[] triangles)
        {
            mesh = new TriangleMesh(triangles);
            tree = default;
        }

        public readonly RayHit? Intersect(in Ray ray)
        {
            return tree.Intersect(in ray);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            mesh.Apply(in matrix);
            tree = new KdTree(mesh.triangles);
        }
    }
}
