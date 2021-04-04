namespace CowLibrary
{
    using System.Collections.Generic;

    public class TriangleMesh : Mesh
    {
        private readonly List<Triangle> triangles;
        
        public TriangleMesh(List<Triangle> triangles)
        {
            this.triangles = triangles;
        }
        
        public override bool Intersect(Ray ray, out Surfel surfel)
        {
            surfel = null;
            return false;
        }
    }
}