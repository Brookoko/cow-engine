namespace CowLibrary
{
    using System.Collections.Generic;

    public class KdNode
    {
        public readonly TriangleMesh mesh;
        public readonly List<KdNode> children = new();

        public KdNode(List<Triangle> triangles)
        {
            mesh = new TriangleMesh(triangles);
        }

        public bool Intersect(Ray ray, out Surfel surfel)
        {
            if (children.Count == 0 && mesh.triangles.Count == 0)
            {
                surfel = null;
                return false;
            }
            if (!mesh.BoundingBox.Intersect(ray, out surfel))
            {
                surfel = null;
                return false;
            }
            return children.Count > 0 ? IntersectChildren(ray, out surfel) : mesh.Intersect(ray, out surfel);
        }

        private bool IntersectChildren(Ray ray, out Surfel surfel)
        {
            surfel = null;
            var intersected = false;
            foreach (var node in children)
            {
                if (node.Intersect(ray, out var s))
                {
                    if (surfel == null || surfel.t > s.t)
                    {
                        intersected = true;
                        surfel = s;
                    }
                }
            }
            return intersected;
        }
    }
}
