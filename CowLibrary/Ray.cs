namespace CowLibrary
{
    using System.Numerics;

    public readonly struct Ray
    {
        public readonly Vector3 origin;
        public readonly Vector3 direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction.Normalize();
        }

        public Vector3 GetPoint(float distance)
        {
            return origin + distance * direction;
        }
    }
}
