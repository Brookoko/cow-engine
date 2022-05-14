namespace CowLibrary
{
    using System.Numerics;
    using Models;
    using Mathematics.Sampler;

    public class OrthographicCamera : Camera
    {
        public override ICameraModelLocal Model => model;

        private readonly OrthographicCameraModel model;

        public OrthographicCamera(int width, int height, ISampler sampler) : base(width, height, sampler)
        {
            model = new OrthographicCameraModel(Width, Height);
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
