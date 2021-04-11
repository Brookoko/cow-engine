namespace CowLibrary
{
    using System.Numerics;
    using Processors;

    public class Sphere : Mesh
    {
        public readonly float radius;
        public readonly Vector3 center;
        
        public override Box BoundingBox { get; }

        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
            BoundingBox = new Box(center, radius * 2);
        }

        public override bool Intersect(Ray ray, out Surfel surfel) => 
            SphereIntersectionProcessor.CheckForIntersection(this, ray, out surfel);
    }
}