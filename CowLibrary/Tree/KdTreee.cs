namespace CowLibrary
{
    public readonly struct KdTree : IIntersectable
    {
        public int Id => root.Id;

        public readonly KdNode[] nodes;
        private readonly KdNode root;

        public KdTree(KdNode[] nodes) : this()
        {
            this.nodes = nodes;
            root = nodes[0];
        }

        public void Intersect(in Ray ray, ref RayHit best)
        {
            root.Intersect(in ray, in nodes, ref best);
        }
    }
}
