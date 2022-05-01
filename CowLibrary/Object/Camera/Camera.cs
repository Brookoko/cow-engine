namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Numerics;

    public abstract class Camera : SceneObject
    {
        public float AspectRatio => (float) width / height;
        
        public int width;
        public int height;
        
        public float nearPlane = 1f;
        public float farPlane = 500f;

        public abstract Ray ScreenPointToRay(Vector2 screenPoint);
        
        public abstract List<Ray> Sample(Vector2 screenPoint, int samples);
    }
}