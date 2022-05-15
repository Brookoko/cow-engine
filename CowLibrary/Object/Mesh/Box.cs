namespace CowLibrary
{
    using System;
    using System.Numerics;

    public struct Box : IMesh
    {
        public Bound BoundingBox { get; }

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
            BoundingBox = new Bound(min, max);
        }

        public Box(Vector3 size)
        {
            this.size = size;
            center = Vector3.Zero;
            min = center - size;
            max = center + size;
            BoundingBox = new Bound(min, max);
        }

        public readonly RayHit? Intersect(in Ray ray)
        {
            var surfel = BoundingBox.Intersect(in ray);
            if (!surfel.HasValue)
            {
                return null;
            }
            return surfel.Value with { normal = GetNormal(surfel.Value.point) };
        }

        private readonly Vector3 GetNormal(in Vector3 point)
        {
            var localPoint = point - center;
            var min = Math.Abs(size.X - Math.Abs(localPoint.X));
            var normal = Vector3.UnitX;
            normal *= Math.Sign(localPoint.X);

            var dist = Math.Abs(size.Y - Math.Abs(localPoint.Y));
            if (dist < min)
            {
                min = dist;
                normal = Vector3.UnitY;
                normal *= Math.Sign(localPoint.Y);
            }
            dist = Math.Abs(size.Z - Math.Abs(localPoint.Z));
            if (dist < min)
            {
                min = dist;
                normal = Vector3.UnitZ;
                normal *= Math.Sign(localPoint.Z);
            }
            return normal;
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
