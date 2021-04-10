namespace CowLibrary
{
    using System.Collections.Generic;

    public class KdNode
    {
        public readonly TriangleMesh mesh;
        public readonly List<KdNode> children = new List<KdNode>();
        
        public KdNode(List<Triangle> triangles)
        {
            mesh = new TriangleMesh(triangles);
        }
        
        public bool Intersect(Ray ray, out Surfel surfel)
        {
            if (children.Count > 0)
            {
                return IntersectChildren(ray, out surfel);
            }
            if (mesh.BoundingBox.Intersect(ray, out _))
            {
                return mesh.Intersect(ray, out surfel);
            }
            surfel = null;
            return false;
        }
        
        private bool IntersectChildren(Ray ray, out Surfel surfel)
        {
            surfel = null;
            var intersected = false;
            foreach (var node in children)
            {
                if (node.Intersect(ray, out var s))
                {
                    intersected = true;
                    if (surfel == null || surfel.t > s.t)
                    {
                        surfel = s;
                    }
                }
            }
            return intersected;
        }
    }
}