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

        public RayHit Intersect(in Ray ray)
        {
            return root.Intersect(in ray, in nodes);
        }
    }
}
