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

        public readonly RayHit Intersect(in Ray ray)
        {
            var hit = view.Intersect(in ray);
            return new RayHit(hit.t, hit.point, GetNormal(hit.point), Id);
        }

        private readonly Vector3 GetNormal(in Vector3 point)
        {
            var localPoint = point - view.center;
            var min = Math.Abs(view.size.X - Math.Abs(localPoint.X));
            var normal = localPoint.X >= 0 ? Vector3.UnitX : -Vector3.UnitX;

            var dist = Math.Abs(view.size.Y - Math.Abs(localPoint.Y));
            if (dist < min)
            {
                min = dist;
                normal = localPoint.Y >= 0 ? Vector3.UnitY : -Vector3.UnitY;
            }
            dist = Math.Abs(view.size.Z - Math.Abs(localPoint.Z));
            if (dist < min)
            {
                min = dist;
                normal = localPoint.Z >= 0 ? Vector3.UnitZ : -Vector3.UnitZ;
            }
            return normal;
        }

        public void Apply(in Matrix4x4 matrix)
        {
            var center = matrix.MultiplyPoint(view.center);
            var size = matrix.MultiplyVector(view.size);
            var min = center - size;
            var max = center + size;
            view = new Bound(min, max, Id);
        }
    }
}
