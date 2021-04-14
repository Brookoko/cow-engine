namespace CowLibrary
{
    using System.Numerics;

    public abstract class Camera
    {
        public Transform transform;

        public int xResolution;
        public int yReslution;

        public float AspectRatio => xResolution / yReslution;
        
        public float nearPlane = 0.1f;
        public float farPlane = 500f;

        public abstract Ray ScreenPointToRay(Vector2 screenPoint);
    }
}