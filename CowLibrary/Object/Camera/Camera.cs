namespace CowLibrary
{
    using System.Numerics;
    using Models;
    using Mathematics.Sampler;

    public abstract class Camera : SceneObject
    {
        public abstract ICameraModelLocal Model { get; }

        public float AspectRatio => (float)Width / Height;

        public int Width { get; }
        public int Height { get; }

        protected readonly ISampler sampler;

        public Camera(int width, int height, ISampler sampler)
        {
            this.sampler = sampler;
            Width = width;
            Height = height;
        }

        public abstract Ray ScreenPointToRay(in Vector2 screenPoint);

        public abstract Ray[] Sample(in Vector2 screenPoint, int samples);
    }
}
