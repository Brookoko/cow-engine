namespace CowLibrary
{
    using System.Numerics;

    public struct OptimizedMesh : IMesh
    {
        public Box BoundingBox => mesh.BoundingBox;

        private readonly TriangleMesh mesh;
        private KdTree tree;

        public OptimizedMesh(Triangle[] triangles)
        {
            mesh = new TriangleMesh(triangles);
            tree = null;
        }

        public readonly Surfel? Intersect(in Ray ray)
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
