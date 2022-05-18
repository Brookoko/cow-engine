namespace CowLibrary
{
    using System.Numerics;

    public interface IMesh : IIntersectable
    {
        public int Id { get; }

        public Bound GetBoundingBox();

        public void Apply(in Matrix4x4 matrix);
    }

    public interface IIntersectable
    {
        public RayHit Intersect(in Ray ray);
    }
}
