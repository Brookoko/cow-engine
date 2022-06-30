namespace CowLibrary
{
    using System.Numerics;

    public readonly struct Ray
    {
        public readonly Vector3 origin;
        public readonly Vector3 direction;
        public readonly Vector3 invDirection;

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction.Normalize();
            invDirection = new Vector3(1 / direction.X, 1 / direction.Y, 1 / direction.Z);
        }

        public Vector3 GetPoint(float distance)
        {
            return origin + distance * direction;
        }
    }
}
