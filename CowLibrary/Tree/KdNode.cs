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

        public void Intersect(in Ray ray, in KdNode[] nodes, ref RayHit best)
        {
            if (!bound.Check(in ray, out var t) || t > best.t)
            {
                return;
            }
            if (leftIndex >= 0)
            {
                IntersectChildren(in ray, in nodes, ref best);
            }
            else
            {
                mesh.Intersect(in ray, ref best);
            }
        }

        private void IntersectChildren(in Ray ray, in KdNode[] nodes, ref RayHit best)
        {
            nodes[rightIndex].Intersect(in ray, in nodes, ref best);
            nodes[middleIndex].Intersect(in ray, in nodes, ref best);
            nodes[leftIndex].Intersect(in ray, in nodes, ref best);
        }

        public KdNode Copy(int leftIndex, int middleIndex, int rightIndex)
        {
            return new KdNode(mesh, bound, leftIndex, middleIndex, rightIndex);
        }
    }
}
