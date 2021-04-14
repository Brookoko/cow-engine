namespace CowLibrary
{
    using System.Numerics;

    public class OrthographicCamera : Camera
    {
        private float horizontalSize;

        public float HorizontalSize
        {
            get => horizontalSize;
            set => horizontalSize = value;
        }

        public float VerticalSize
        {
            get => horizontalSize / AspectRatio;
            set => horizontalSize = value * AspectRatio;
        }

        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            var xOffset = (screenPoint.X / xResolution - 0.5f) * horizontalSize;
            var yOffset = (screenPoint.Y / yReslution - 0.5f) * VerticalSize;
            var origin = transform.position + transform.Up * new Vector3(xOffset, yOffset, 0);
            return new Ray(origin, transform.Forward);
        }
    }
}