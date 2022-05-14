namespace CowLibrary
{
    using System.Numerics;
    using Models;

    public class OrthographicCamera : Camera
    {
        public override ICameraModel Model => model;

        private readonly OrthographicCameraModel model;

        public OrthographicCamera(int width, int height) : base(width, height)
        {
            model = new OrthographicCameraModel(Width, Height);
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
