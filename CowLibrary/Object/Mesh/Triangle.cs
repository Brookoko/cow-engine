namespace CowLibrary
{
    using System.Numerics;

    public class Triangle : Mesh
    {
        public readonly Vector3 v0;
        public readonly Vector3 v1;
        public readonly Vector3 v2;
        
        public Vector3 n0;
        public Vector3 n1;
        public Vector3 n2;
        
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }
        
        public void SetNormal(Vector3 n0, Vector3 n1, Vector3 n2)
        {
            this.n0 = n0;
            this.n1 = n1;
            this.n2 = n2;
        }
        
        public void CalculateNormal()
        {
            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;
            var n = Vector3.Cross(v0v2, v0v1);
            n0 = n1 = n2 = n;
        }
        
        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            surfel = null;
            return false;
        }
    }
}