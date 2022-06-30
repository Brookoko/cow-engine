namespace CowLibrary
{
    using System.Numerics;
    using Object.Mesh.Views;

    public struct Plane : IMesh<PlaneView>
    {
        public readonly int Id => view.Id;

        public readonly PlaneView View => view;

        public readonly Bound BoundingBox => bound;

        private Bound bound;
        private PlaneView view;

        public Plane(int id) : this()
        {
            view = new PlaneView(Vector3.Zero, Vector3.UnitY, id);
            bound = CreateBound();
        }

        private Bound CreateBound()
        {
            return new Bound(view.point, 1000, Id);
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            view.Intersect(in ray, ref best);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            var point = matrix.MultiplyPoint(view.point);
            var normal = matrix.MultiplyVector(view.normal).Normalize();
            view = new PlaneView(point, normal, Id);
            bound = CreateBound();
        }
    }
}
