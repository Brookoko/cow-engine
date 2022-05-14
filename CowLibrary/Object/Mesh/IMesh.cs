namespace CowLibrary
{
    using System.Numerics;

    public interface IMesh : IIntersectable
    {
        public Box BoundingBox { get; }

        public void Apply(in Matrix4x4 matrix);
    }

    public interface IIntersectable
    {
        public Surfel? Intersect(in Ray ray);
    }
}
