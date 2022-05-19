namespace CowLibrary
{
    using System.Numerics;

    public interface IMesh : IIntersectable
    {
        public IIntersectable View { get; }

        public Bound GetBoundingBox();

        public void Apply(in Matrix4x4 matrix);
    }

    public interface IIntersectable
    {
        public int Id { get; }

        public RayHit Intersect(in Ray ray);
    }
}
