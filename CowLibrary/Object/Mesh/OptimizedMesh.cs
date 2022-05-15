namespace CowLibrary
{
    using System.Numerics;

    public struct OptimizedMesh : IMesh
    {
        public readonly TriangleMesh mesh;
        private KdTree tree;

        public OptimizedMesh(Triangle[] triangles)
        {
            mesh = new TriangleMesh(triangles);
            tree = default;
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
            tree = KdTreeBuilder.Build(mesh.triangles);
        }
    }
}
