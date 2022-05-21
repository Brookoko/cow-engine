namespace CowLibrary
{
    using System.Numerics;

    public interface IMesh<out T> : IMesh where T : IIntersectable
    {
        public T View { get; }
    }

    public interface IMesh : IIntersectable
    {
        public Bound BoundingBox { get; }

        public void Apply(in Matrix4x4 matrix);
    }

    public interface IIntersectable
    {
        public int Id { get; }

        public void Intersect(in Ray ray, ref RayHit best);
    }
}
