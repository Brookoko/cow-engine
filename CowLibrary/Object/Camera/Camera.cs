namespace CowLibrary
{
    using System.Linq;
    using System.Numerics;
    using Models;
    using Mathematics.Sampler;

    public abstract class Camera : SceneObject
    {
        public abstract ICameraModel Model { get; }

        public float AspectRatio => (float)Width / Height;

        public int Width { get; }
        public int Height { get; }

        private readonly ISampler sampler;

        protected Camera(int width, int height, ISampler sampler)
        {
            this.sampler = sampler;
            Width = width;
            Height = height;
        }

        public Ray ScreenPointToRay(in Vector2 screenPoint)
        {
            return Model.ScreenPointToRay(in screenPoint, Transform.LocalToWorldMatrix, sampler.CreateSample());
        }

        public Ray[] Sample(in Vector2 screenPoint, int samples)
        {
            var generatedSamples = new Vector2[samples];
            for (var i = 0; i < samples; i++)
            {
                generatedSamples[i] = sampler.CreateSample();
            }
            return Model.Sample(in screenPoint, Transform.LocalToWorldMatrix, in generatedSamples);
        }
    }
}
