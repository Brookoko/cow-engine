namespace CowLibrary
{
    public readonly struct KdTree : IIntersectable
    {
        public int Id => nodes[0].mesh.Id;

        public readonly KdNode[] nodes;

        public KdTree(KdNode[] nodes) : this()
        {
            this.nodes = nodes;
        }

        public RayHit Intersect(in Ray ray)
        {
            return nodes[0].Intersect(in ray, in nodes);
        }
    }
}
