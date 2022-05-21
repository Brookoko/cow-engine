namespace CowLibrary
{
    public readonly struct KdNode
    {
        public readonly TriangleMesh mesh;
        public readonly Bound bound;
        public readonly int leftIndex;
        public readonly int middleIndex;
        public readonly int rightIndex;

        public int Id => mesh.Id;

        public KdNode(Triangle[] triangles, int id)
        {
            mesh = new TriangleMesh(triangles, id);
            bound = mesh.BoundingBox;
            leftIndex = -1;
            middleIndex = -1;
            rightIndex = -1;
        }

        public KdNode(in Triangle[] triangles, int leftIndex, int middleIndex, int rightIndex)
        {
            mesh = new TriangleMesh();
            bound = IntersectionHelper.CreateBound(triangles, -1);
            this.leftIndex = leftIndex;
            this.middleIndex = middleIndex;
            this.rightIndex = rightIndex;
        }

        private KdNode(TriangleMesh mesh, Bound bound, int leftIndex, int middleIndex, int rightIndex)
        {
            this.mesh = mesh;
            this.bound = bound;
            this.leftIndex = leftIndex;
            this.middleIndex = middleIndex;
            this.rightIndex = rightIndex;
        }

        public RayHit Intersect(in Ray ray, in KdNode[] nodes)
        {
            var boundHit = bound.Intersect(in ray);
            if (!boundHit.HasHit)
            {
                return boundHit;
            }
            return leftIndex >= 0 ? IntersectChildren(in ray, in nodes) : mesh.Intersect(in ray);
        }

        private RayHit IntersectChildren(in Ray ray, in KdNode[] nodes)
        {
            var hit = Const.Miss;
            IntersectChild(in ray, in nodes, in nodes[leftIndex], ref hit);
            IntersectChild(in ray, in nodes, in nodes[middleIndex], ref hit);
            IntersectChild(in ray, in nodes, in nodes[rightIndex], ref hit);
            return hit;
        }

        private void IntersectChild(in Ray ray, in KdNode[] nodes, in KdNode child, ref RayHit hit)
        {
            var cHit = child.Intersect(in ray, in nodes);
            if (hit.t > cHit.t)
            {
                hit = cHit;
            }
        }

        public KdNode Copy(int leftIndex, int middleIndex, int rightIndex)
        {
            return new KdNode(mesh, bound, leftIndex, middleIndex, rightIndex);
        }
    }
}
