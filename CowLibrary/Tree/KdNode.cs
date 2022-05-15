namespace CowLibrary
{
    public readonly struct KdNode
    {
        private const int ChildCount = 3;
        
        public readonly TriangleMesh mesh;
        public readonly Bound bound;
        public readonly int index;

        public KdNode(Triangle[] triangles)
        {
            mesh = new TriangleMesh(triangles);
            bound = mesh.GetBoundingBox();
            index = -1;
        }

        public KdNode(in Triangle[] triangles, int index)
        {
            mesh = new TriangleMesh();
            bound = IntersectionHelper.CreateBound(triangles);
            this.index = index;
        }

        public RayHit Intersect(in Ray ray, in KdNode[] nodes)
        {
            if (!HasChildren() && mesh.triangles.Length == 0)
            {
                return new RayHit();
            }
            var boundHit = bound.Intersect(in ray);
            if (!boundHit.HasHit)
            {
                return boundHit;
            }
            return HasChildren() ? IntersectChildren(in ray, in nodes) : mesh.Intersect(in ray);
        }

        private bool HasChildren()
        {
            return index >= 0;
        }

        private RayHit IntersectChildren(in Ray ray, in KdNode[] nodes)
        {
            var hit = new RayHit();
            for (var i = 1; i <= ChildCount; i++)
            {
                var child = nodes[ChildCount * index + i];
                var cHit = child.Intersect(in ray, in nodes);
                if (hit.t > cHit.t)
                {
                    hit = cHit;
                }
            }
            return hit;
        }
    }
}
