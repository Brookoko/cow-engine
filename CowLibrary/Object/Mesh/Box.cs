namespace CowLibrary
{
    using System;
    using System.Numerics;

    public struct Box : IMesh<Box>
    {
        public readonly int Id => view.Id;

        public readonly Box View => this;

        public readonly Bound BoundingBox => view;

        private Bound view;

        public Box(Vector3 size, int id)
        {
            view = new Bound(-size, size, id);
        }

        public readonly void Intersect(in Ray ray, ref RayHit best)
        {
            var prevBest = best;
            view.Intersect(in ray, ref best);
            if (best.t < prevBest.t)
            {
                var (n, dpdu, dpdv) = GetBasis(best.point);
                best = new RayHit(best.t, best.point, n, dpdu, dpdv, Id);
            }
        }

        private readonly (Vector3 normal, Vector3 dpdu, Vector3 dpdv) GetBasis(in Vector3 point)
        {
            var localPoint = point - view.Center;
            var min = Math.Abs(view.Size.X - Math.Abs(localPoint.X));
            var sign = localPoint.X >= 0 ? 1 : -1;
            var normal = sign * Vector3.UnitX;
            var dpdu = sign * Vector3.UnitZ;
            var dpdv = sign * Vector3.UnitY;

            var dist = Math.Abs(view.Size.Y - Math.Abs(localPoint.Y));
            if (dist < min)
            {
                min = dist;
                sign = localPoint.Y >= 0 ? 1 : -1;
                normal = sign * Vector3.UnitY;
                dpdu = sign * Vector3.UnitZ;
                dpdv = sign * Vector3.UnitX;
            }
            dist = Math.Abs(view.Size.Z - Math.Abs(localPoint.Z));
            if (dist < min)
            {
                min = dist;
                sign = localPoint.Y >= 0 ? 1 : -1;
                normal = sign * Vector3.UnitZ;
                dpdu = sign * Vector3.UnitY;
                dpdv = sign * Vector3.UnitX;
            }
            return (normal, dpdu, dpdv);
        }

        public void Apply(in Matrix4x4 matrix)
        {
            var center = matrix.MultiplyPoint(view.Center);
            var size = matrix.MultiplyVector(view.Size);
            var min = center - size;
            var max = center + size;
            view = new Bound(min, max, Id);
        }
    }
}
