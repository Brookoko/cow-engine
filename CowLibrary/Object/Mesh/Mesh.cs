namespace CowLibrary
{
    using System.Numerics;

    public abstract class Mesh
    {
        public abstract Box BoundingBox { get; }

        public abstract bool Intersect(Ray ray, out Surfel surfel);

        public abstract void Apply(Matrix4x4 matrix);
    }
}
