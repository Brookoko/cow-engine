namespace CowLibrary
{
    using System.Numerics;

    public struct Disk : IMesh
    {
        public Bound BoundingBox { get; private set; }

        private Vector3 normal;
        private Vector3 point;
        private float radius;

        public Disk(float radius) : this()
        {
            this.radius = radius;
            point = Vector3.Zero;
            normal = -Vector3.UnitY;
            BoundingBox = CreateBound();
        }
        
        private Bound CreateBound()
        {
            return new Bound(point, 2 * radius);
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
            var p = ray.GetPoint(t);
            var dist = Vector3.DistanceSquared(p, point);
            if (dist > radius * radius)
            {
                return null;
            }
            return new Surfel()
            {
                t = t,
                point = p,
                normal = -normal
            };
        }

        public void Apply(in Matrix4x4 matrix)
        {
            point = matrix.MultiplyPoint(point);
            radius = matrix.ExtractScale().Min() * radius;
            normal = matrix.MultiplyVector(normal).Normalize();
            BoundingBox = CreateBound();
        }
    }
}
