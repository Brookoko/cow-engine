namespace CowLibrary
{
    using System.Collections.Generic;

    public class OptimizedMesh : Mesh
    {
        public override Box BoundingBox { get; }
        
        private readonly KdTree tree;
        
        public OptimizedMesh(List<Triangle> triangles)
        {
            tree = new KdTree(triangles);
            BoundingBox = tree.Box;
        }
        
        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            return tree.Intersect(ray, out surfel);
        }
    }
}