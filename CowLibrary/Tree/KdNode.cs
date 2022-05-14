namespace CowLibrary
{
    public readonly struct KdNode : IIntersectable
    {
        public readonly TriangleMesh mesh;
        public readonly KdNode[] children = new KdNode[3];

        public KdNode(Triangle[] triangles)
        {
            mesh = new TriangleMesh(triangles);
        }

        public Surfel? Intersect(in Ray ray)
        {
            if (children.Length == 0 && mesh.triangles.Length == 0)
            {
                return null;
            }
            var surfel = mesh.BoundingBox.Intersect(in ray);
            if (!surfel.HasValue)
            {
                return null;
            }
            return children.Length > 0 ? IntersectChildren(in ray) : mesh.Intersect(in ray);
        }

        private Surfel? IntersectChildren(in Ray ray)
        {
            Surfel? surfel = null;
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
