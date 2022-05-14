namespace CowLibrary
{
    using System.Numerics;
    using Models;
    using Mathematics.Sampler;

    public class PerspectiveCamera : Camera
    {
        public double Fov { get; }

        public override ICameraModelLocal Model => model;

        private readonly PerspectiveCameraModel model;

        public PerspectiveCamera(int width, int height, ISampler sampler, float fov) : base(width, height, sampler)
        {
            Fov = fov;
            model = new PerspectiveCameraModel(width, height, fov, 1);
        }

        public override Ray ScreenPointToRay(in Vector2 screenPoint)
        {
            return model.ScreenPointToRay(in screenPoint, Transform.LocalToWorldMatrix, sampler);
        }

        public override Ray[] Sample(in Vector2 screenPoint, int samples)
        {
            return model.Sample(in screenPoint, Transform.LocalToWorldMatrix, sampler, samples);
        }
    }
}
