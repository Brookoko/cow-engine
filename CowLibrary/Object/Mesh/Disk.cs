namespace CowLibrary
{
    using System.Numerics;

    public struct Disk : IMesh
    {
        private Bound bound;
        private Vector3 normal;
        private Vector3 point;
        private float radius;

        public int Id { get; }
        
        public Disk(float radius, int id) : this()
        {
            this.radius = radius;
            point = Vector3.Zero;
            normal = -Vector3.UnitY;
            bound = CreateBound();
            Id = id;
        }

        private Bound CreateBound()
        {
            return new Bound(point, 2 * radius);
        }

        public readonly RayHit Intersect(in Ray ray)
        {
            var dot = Vector3.Dot(normal, ray.direction);
            if (dot <= Const.Epsilon)
            {
                return Const.Miss;
            }
            var dir = point - ray.origin;
            var t = Vector3.Dot(dir, normal) / dot;
            if (t <= 0)
            {
                return Const.Miss;
            }
            var p = ray.GetPoint(t);
            var dist = Vector3.DistanceSquared(p, point);
            if (dist > radius * radius)
            {
                return Const.Miss;
            }
            return new RayHit(t, p, -normal, Id);
        }

        public readonly Bound GetBoundingBox()
        {
            return bound;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            point = matrix.MultiplyPoint(point);
            radius = matrix.ExtractScale().Min() * radius;
            normal = matrix.MultiplyVector(normal).Normalize();
            bound = CreateBound();
        }
    }
}
