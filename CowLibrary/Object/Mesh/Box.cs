namespace CowLibrary
{
    using System;
    using System.Numerics;

    public struct Box : IMesh
    {
        private readonly Bound bound;
        private Vector3 center;
        private Vector3 min;
        private Vector3 max;
        private Vector3 size;

        public Box(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
            center = new Vector3((min.X + max.X) * 0.5f, (min.Y + max.Y) * 0.5f, (min.Z + max.Z) * 0.5f);
            size = max - center;
            bound = new Bound(min, max);
        }

        public Box(Vector3 size)
        {
            this.size = size;
            center = Vector3.Zero;
            min = center - size;
            max = center + size;
            bound = new Bound(min, max);
        }

        public readonly RayHit Intersect(in Ray ray)
        {
            var hit = bound.Intersect(in ray);
            return new RayHit(hit.t, hit.point, GetNormal(hit.point));
        }

        private readonly Vector3 GetNormal(in Vector3 point)
        {
            var localPoint = point - center;
            var min = Math.Abs(size.X - Math.Abs(localPoint.X));
            var normal = localPoint.X >= 0 ? Vector3.UnitX : -Vector3.UnitX;

            var dist = Math.Abs(size.Y - Math.Abs(localPoint.Y));
            if (dist < min)
            {
                min = dist;
                normal = localPoint.Y >= 0 ? Vector3.UnitY : -Vector3.UnitY;
            }
            dist = Math.Abs(size.Z - Math.Abs(localPoint.Z));
            if (dist < min)
            {
                min = dist;
                normal = localPoint.Z >= 0 ? Vector3.UnitZ : -Vector3.UnitZ;
            }
            return normal;
        }

        public readonly Bound GetBoundingBox()
        {
            return bound;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            center = matrix.MultiplyPoint(center);
            size = matrix.MultiplyVector(size);
            min = center - size;
            max = center + size;
        }
    }
}
