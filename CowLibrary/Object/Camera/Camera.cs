namespace CowLibrary
{
    using System.Numerics;

    public abstract class Camera
    {
        public float AspectRatio => (float) width / height;
        
        public Transform transform = new Transform();

        public int width;
        public int height;
        
        public float nearPlane = 1f;
        public float farPlane = 500f;

        public abstract Ray ScreenPointToRay(Vector2 screenPoint);
    }
}