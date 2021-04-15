namespace CowLibrary
{
    using System.Numerics;

    public class Box : Mesh
    {
        public Vector3 center;
        public Vector3 min;
        public Vector3 max;

        public override Box BoundingBox => this;

        public Box(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
            center = new Vector3((min.X + max.X) * 0.5f, (min.Y + max.Y) * 0.5f, (min.Z + max.Z) * 0.5f);
        }

        public Box(Vector3 center, float sideLength)
        {
            this.center = center;
            var diagonalHalf = new Vector3(sideLength / 2);
            min = center - diagonalHalf;
            max = center + diagonalHalf;
        }

        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            surfel = null;
            return false;
        }
        
        public override void Apply(Matrix4x4 matrix)
        {
            center = matrix.MultiplyPoint(center);
            max = matrix.MultiplyVector(max);
            min = matrix.MultiplyVector(min);
        }
    }
}