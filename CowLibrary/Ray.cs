namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class Ray
    {
        public readonly Vector3 origin;
        public readonly Vector3 direction;
        
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            if (direction == Vector3.Zero)
            {
                throw new ArgumentException($"Ray direction must be non-zero vector.");
            }
            
            this.direction = direction.Normalize();
        }
        
        public Vector3 GetPoint(float distance)
        {
            return origin + distance * direction;
        }
    }
}