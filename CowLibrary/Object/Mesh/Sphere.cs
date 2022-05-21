namespace CowLibrary
{
    using System.Numerics;
    using Object.Mesh.Views;

    public struct Sphere : IMesh<SphereView>
    {
        public readonly int Id => view.Id;

        public readonly SphereView View => view;

        public readonly Bound BoundingBox => bound;

        private Bound bound;
        private SphereView view;

        public Sphere(float radius, int id) : this()
        {
            view = new SphereView(Vector3.Zero, radius, id);
            bound = CreateBound();
        }

        private Bound CreateBound()
        {
            return new Bound(view.center, view.radius * 2, Id);
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            view.Intersect(in ray, ref best);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            var center = matrix.MultiplyPoint(view.center);
            var radius = matrix.ExtractScale().Min() * view.radius;
            view = new SphereView(center, radius, Id);
            bound = CreateBound();
        }
    }
}
