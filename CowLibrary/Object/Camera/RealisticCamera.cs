namespace CowLibrary
{
    using System.Numerics;
    using Models;
    using Mathematics.Sampler;

    public class RealisticCamera : Camera
    {
        public double Fov { get; }

        public override ICameraModelLocal Model => model;

        private readonly RealisticCameraModel model;

        public RealisticCamera(int width, int height, ISampler sampler, float fov, Lens lens) : base(width, height, sampler)
        {
            Fov = fov;
            model = new RealisticCameraModel(width, height, fov, lens);
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
