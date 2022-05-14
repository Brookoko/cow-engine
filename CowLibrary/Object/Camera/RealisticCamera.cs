namespace CowLibrary
{
    using System.Numerics;
    using Models;

    public class RealisticCamera : Camera
    {
        public double Fov { get; }

        public override ICameraModel Model => model;

        private readonly RealisticCameraModel model;

        public RealisticCamera(int width, int height, float fov, Lens lens) : base(width, height)
        {
            Fov = fov;
            model = new RealisticCameraModel(width, height, fov, lens);
        }

        public override Ray ScreenPointToRay(in Vector2 screenPoint)
        {
            return model.ScreenPointToRay(in screenPoint, Transform.LocalToWorldMatrix);
        }

        public override Ray[] Sample(in Vector2 screenPoint, int samples)
        {
            return model.Sample(in screenPoint, Transform.LocalToWorldMatrix, samples);
        }
    }
}
