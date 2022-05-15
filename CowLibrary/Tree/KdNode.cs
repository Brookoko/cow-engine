namespace CowLibrary
{
    public readonly struct KdNode : IIntersectable
    {
        public readonly TriangleMesh mesh;
        public readonly KdNode[] children;
        public readonly Bound bound;

        public KdNode(Triangle[] triangles)
        {
            mesh = new TriangleMesh(triangles);
            children = new KdNode[0];
            bound = mesh.BoundingBox;
        }

        public KdNode(in Triangle[] triangles, KdNode leftNode, KdNode middleNode, KdNode rightNode)
        {
            mesh = default;
            bound = IntersectionHelper.CreateBound(triangles);
            children = new KdNode[3];
            children[0] = leftNode;
            children[1] = middleNode;
            children[2] = rightNode;
        }

        public RayHit? Intersect(in Ray ray)
        {
            if (children.Length == 0 && mesh.triangles.Length == 0)
            {
                return null;
            }
            var surfel = bound.Intersect(in ray);
            if (!surfel.HasValue)
            {
                return null;
            }
            return HasChildren() ? IntersectChildren(in ray) : mesh.Intersect(in ray);
        }

        private bool HasChildren()
        {
            return children.Length > 0;
        }

        private RayHit? IntersectChildren(in Ray ray)
        {
            RayHit? surfel = null;
            foreach (var child in children)
            {
                var s = child.Intersect(in ray);
                if (s.HasValue)
                {
                    if (!surfel.HasValue || surfel.Value.t > s.Value.t)
                    {
                        surfel = s;
                    }
                }
            }
            return surfel;
        }
    }
}
