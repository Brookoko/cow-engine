namespace CowLibrary
{
    using Models;
    using Mathematics.Sampler;

    public class OrthographicCamera : Camera
    {
        public override ICameraModel Model => model;

        private readonly OrthographicCameraModel model;

        public OrthographicCamera(int width, int height, ISampler sampler) : base(width, height, sampler)
        {
            model = new OrthographicCameraModel(Width, Height);
        }
    }
}
