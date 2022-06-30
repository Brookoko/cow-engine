namespace CowLibrary
{
    using Models;
    using Mathematics.Sampler;

    public class PerspectiveCamera : Camera
    {
        public double Fov { get; }

        public override ICameraModel Model => model;

        private readonly PerspectiveCameraModel model;

        public PerspectiveCamera(int width, int height, ISampler sampler, float fov) : base(width, height, sampler)
        {
            Fov = fov;
            model = new PerspectiveCameraModel(width, height, fov, 1);
        }
    }
}
