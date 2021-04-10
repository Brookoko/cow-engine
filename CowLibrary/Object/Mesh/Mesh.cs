namespace CowLibrary
{
    public abstract class Mesh
    {
        public abstract Box BoundingBox { get; }
        
        public abstract bool Intersect(Ray ray, out Surfel surfel);
    }
}