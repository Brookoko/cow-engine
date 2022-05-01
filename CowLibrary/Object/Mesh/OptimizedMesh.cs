namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Numerics;

    public class OptimizedMesh : Mesh
    {
        public override Box BoundingBox => mesh.BoundingBox;

        private readonly TriangleMesh mesh;
        private KdTree tree;

        public OptimizedMesh(List<Triangle> triangles)
        {
            mesh = new TriangleMesh(triangles);
        }

        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            return tree.Intersect(ray, out surfel);
        }

        public override void Apply(Matrix4x4 matrix)
        {
            mesh.Apply(matrix);
            tree = new KdTree(mesh.triangles);
        }
    }
}
