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
                best = new RayHit(best.t, best.point, GetNormal(best.point), Id);
            }
        }

        private readonly Vector3 GetNormal(in Vector3 point)
        {
            var localPoint = point - view.Center;
            var min = Math.Abs(view.Size.X - Math.Abs(localPoint.X));
            var normal = localPoint.X >= 0 ? Vector3.UnitX : -Vector3.UnitX;

            var dist = Math.Abs(view.Size.Y - Math.Abs(localPoint.Y));
            if (dist < min)
            {
                min = dist;
                normal = localPoint.Y >= 0 ? Vector3.UnitY : -Vector3.UnitY;
            }
            dist = Math.Abs(view.Size.Z - Math.Abs(localPoint.Z));
            if (dist < min)
            {
                min = dist;
                normal = localPoint.Z >= 0 ? Vector3.UnitZ : -Vector3.UnitZ;
            }
            return normal;
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
