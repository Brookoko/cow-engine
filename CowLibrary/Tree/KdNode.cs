namespace CowLibrary
{
    public readonly struct KdNode
    {
        public readonly TriangleMesh mesh;
        public readonly Bound bound;
        public readonly KdNode[] children;

        public KdNode(Triangle[] triangles, int id)
        {
            mesh = new TriangleMesh(triangles, id);
            bound = mesh.GetBoundingBox();
            children = new KdNode[0];
        }

        public KdNode(in Triangle[] triangles, KdNode[] children)
        {
            mesh = new TriangleMesh();
            bound = IntersectionHelper.CreateBound(triangles, -1);
            this.children = children;
        }

        public RayHit Intersect(in Ray ray, in KdNode[] nodes)
        {
            var boundHit = bound.Intersect(in ray);
            if (!boundHit.HasHit)
            {
                return boundHit;
            }
            return children.Length > 0 ? IntersectChildren(in ray, in nodes) : mesh.Intersect(in ray);
        }

        private RayHit IntersectChildren(in Ray ray, in KdNode[] nodes)
        {
            var hit = Const.Miss;
            foreach (var child in children)
            {
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
