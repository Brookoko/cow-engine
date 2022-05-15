namespace CowLibrary
{
    using System.Numerics;

    public struct Plane : IMesh
    {
        private Bound bound;
        private Vector3 normal;
        private Vector3 point;

        public Plane()
        {
            this = default;
            normal = -Vector3.UnitY;
            point = Vector3.Zero;
            bound = CreateBound();
        }

        private Bound CreateBound()
        {
            return new Bound(point, 1000);
        }

        public readonly RayHit Intersect(in Ray ray)
        {
            var dot = Vector3.Dot(normal, ray.direction);
            if (dot <= Const.Epsilon)
            {
                return new RayHit();
            }
            var dir = point - ray.origin;
            var t = Vector3.Dot(dir, normal) / dot;
            if (t <= 0)
            {
                return new RayHit();
            }
            return new RayHit(t, ray.GetPoint(t), -normal);
        }
        
        public readonly Bound GetBoundingBox()
        {
            return bound;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            normal = matrix.MultiplyVector(normal).Normalize();
            point = matrix.MultiplyPoint(point);
            bound = CreateBound();
        }
    }
}
