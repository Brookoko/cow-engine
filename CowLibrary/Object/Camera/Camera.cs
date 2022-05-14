namespace CowLibrary
{
    using System.Numerics;
    using Models;

    public abstract class Camera : SceneObject
    {
        public abstract ICameraModel Model { get; }

        public float AspectRatio => (float)Width / Height;

        public int Width { get; }
        public int Height { get; }

        public abstract Ray ScreenPointToRay(in Vector2 screenPoint);

        public abstract Ray[] Sample(in Vector2 screenPoint, int samples);

        public Camera(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
