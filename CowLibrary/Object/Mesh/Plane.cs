namespace CowLibrary
{
    using System.Numerics;

    public struct Plane : IMesh
    {
        public Box BoundingBox { get; private set; }

        private Vector3 normal;
        private Vector3 point;

        public Plane()
        {
            this = default;
            normal = -Vector3.UnitY;
            point = Vector3.Zero;
            BoundingBox = CreateBox();
        }

        private Box CreateBox()
        {
            return new Box(point, 1000);
        }

        public readonly Surfel? Intersect(in Ray ray)
        {
            var dot = Vector3.Dot(normal, ray.direction);
            if (dot <= Const.Epsilon)
            {
                return null;
            }
            var dir = point - ray.origin;
            var t = Vector3.Dot(dir, normal) / dot;
            if (t <= 0)
            {
                return null;
            }
            return new Surfel()
            {
                t = t,
                point = ray.GetPoint(t),
                normal = -normal
            };
        }

        public void Apply(in Matrix4x4 matrix)
        {
            normal = matrix.MultiplyVector(normal).Normalize();
            point = matrix.MultiplyPoint(point);
            BoundingBox = CreateBox();
        }
    }
}
