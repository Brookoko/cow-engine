namespace CowLibrary
{
    using System.Numerics;

    public class Disk : Mesh
    {
        private const float e = 1e-10f;

        public override Box BoundingBox => box;

        private Box box;

        private Vector3 normal = -Vector3.UnitY;
        private Vector3 point = Vector3.Zero;
        private float radius;

        public Disk(float radius)
        {
            this.radius = radius;
            box = CreateBox();
        }

        private Box CreateBox()
        {
            return new Box(point, 2 * radius);
        }

        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            var dot = Vector3.Dot(normal, ray.direction);
            if (dot > e)
            {
                var dir = point - ray.origin;
                var t = Vector3.Dot(dir, normal) / dot;
                if (t > 0)
                {
                    var p = ray.GetPoint(t);
                    var dist = Vector3.DistanceSquared(p, point);
                    if (dist <= radius * radius)
                    {
                        surfel = new Surfel()
                        {
                            t = t,
                            point = p,
                            normal = -normal
                        };
                        return true;
                    }
                }
            }
            surfel = null;
            return false;
        }

        public override void Apply(Matrix4x4 matrix)
        {
            point = matrix.MultiplyPoint(point);
            radius = matrix.ExtractScale().Min() * radius;
            normal = matrix.MultiplyVector(normal).Normalize();
            box = CreateBox();
        }
    }
}
