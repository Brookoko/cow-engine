namespace CowLibrary
{
    public readonly struct KdNode
    {
        private const int ChildCount = 3;
        
        public readonly TriangleMesh mesh;
        public readonly Bound bound;
        public readonly int index;

        public KdNode(Triangle[] triangles, int id)
        {
            mesh = new TriangleMesh(triangles, id);
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
            if (index < 0 && mesh.triangles.Length == 0)
            {
                return Const.Miss;
            }
            var boundHit = bound.Intersect(in ray);
            if (!boundHit.HasHit)
            {
                return boundHit;
            }
            return index >= 0 ? IntersectChildren(in ray, in nodes) : mesh.Intersect(in ray);
        }

        private RayHit IntersectChildren(in Ray ray, in KdNode[] nodes)
        {
            var hit = Const.Miss;
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
