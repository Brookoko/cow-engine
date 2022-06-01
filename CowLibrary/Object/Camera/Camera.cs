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

        public Ray[] Sample(in Vector2 screenPoint, int samplesPerDimension)
        {
            var rays = new Ray[samplesPerDimension * samplesPerDimension];
            var step = 1f / (samplesPerDimension + 1);
            var centerIndex = (samplesPerDimension - 1) / 2f;
            for (var i = 0; i < samplesPerDimension; i++)
            {
                for (var j = 0; j < samplesPerDimension; j++)
                {
                    var offset = new Vector2((i - centerIndex) * step + 0.5f, (j - centerIndex) * step + 0.5f);
                    var point = screenPoint + offset;
                    rays[i * samplesPerDimension + j] =
                        Model.ScreenPointToRay(in point, Transform.LocalToWorldMatrix, sampler.CreateSample());
                }
            }
            return rays;
        }
    }
}
