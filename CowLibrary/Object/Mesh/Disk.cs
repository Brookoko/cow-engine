namespace CowLibrary
{
    using System.Numerics;
    using Views;

    public struct Disk : IMesh<DiskView>
    {
        public readonly int Id => view.Id;

        public readonly DiskView View => view;

        public readonly Bound BoundingBox => bound;

        private Bound bound;
        private DiskView view;

        public Disk(float radius, int id) : this()
        {
            view = new DiskView(Vector3.Zero, Vector3.UnitY, radius, id);
            bound = CreateBound();
        }

        private Bound CreateBound()
        {
            return new Bound(view.point, 2 * view.radius, Id);
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            view.Intersect(in ray, ref best);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            var point = matrix.MultiplyPoint(view.point);
            var radius = matrix.ExtractScale().Min() * view.radius;
            var normal = matrix.MultiplyVector(view.normal).Normalize();
            view = new DiskView(point, normal, radius, Id);
            bound = CreateBound();
        }
    }
}
