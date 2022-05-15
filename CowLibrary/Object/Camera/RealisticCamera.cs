namespace CowLibrary
{
    using Models;
    using Mathematics.Sampler;

    public class RealisticCamera : Camera
    {
        public double Fov { get; }

        public override ICameraModel Model => model;

        private readonly RealisticCameraModel model;

        public RealisticCamera(int width, int height, ISampler sampler, float fov, Lens lens) : base(width, height, sampler)
        {
            Fov = fov;
            model = new RealisticCameraModel(width, height, fov, lens);
        }
    }
}
