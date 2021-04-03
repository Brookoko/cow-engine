namespace CowLibrary
{
    public abstract class Mesh
    {
        public abstract bool Intersect(Ray ray, out Surfel surfel);
    }
}