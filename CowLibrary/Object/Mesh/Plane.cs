namespace CowLibrary
{
    using System.Numerics;

    public class Plane : Mesh
    {
        private const float e = 1e-10f;

        public override Box BoundingBox => box;
        
        private Box box;
        
        private Vector3 normal = -Vector3.UnitY;
        private Vector3 point = Vector3.Zero;
        
        public Plane()
        {
            box = CreateBox();
        }
        
        private Box CreateBox()
        {
            return new Box(point, 1000);
        }
        
        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            var dot = Vector3.Dot(normal, ray.direction);
            if (dot > e)
            {
                var dir = point - ray.origin;
                var t = Vector3.Dot(dir, normal) / dot;
                if (t >= 0)
                {
                    surfel = new Surfel()
                    {
                        t = t,
                        point = ray.GetPoint(t),
                        normal = -normal
                    };
                    return true;
                }
            }
            surfel = null;
            return false;
        }
        
        public override void Apply(Matrix4x4 matrix)
        {
            if (Matrix4x4.Invert(matrix, out var m))
            {
                normal = m.MultiplyVector(normal).Normalize();
            }
            point = matrix.MultiplyPoint(point);
            box = CreateBox();
        }
    }
}