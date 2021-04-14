namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class PerspectiveCamera : Camera
    {
        private float horizontalFov;

        private float verticalFov;

        public float HorizontalFov
        {
            get => horizontalFov;
            set => RecalculateFovFromHorizontal(value);
        }

        public float VerticalFov
        {
            get => verticalFov;
            set => RecalculateFovFromVertical(value);
        }

        private void RecalculateFovFromHorizontal(float newHorizontalFov)
        {
            horizontalFov = newHorizontalFov;
            verticalFov = 2 * (float)Math.Atan(Math.Tan(horizontalFov / 2 * MathConstants.Deg2Rad) * yReslution / xResolution) * MathConstants.Rad2Deg;
        }
        
        private void RecalculateFovFromVertical(float newVerticalFov)
        {
            verticalFov = newVerticalFov;
            horizontalFov = 2 * (float) Math.Atan(Math.Tan(verticalFov / 2 * MathConstants.Deg2Rad) * xResolution / yReslution) * MathConstants.Rad2Deg;
        }

        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            return new Ray(transform.position, ScreenToWorldPoint(screenPoint) - transform.position);
        }

        private Vector3 ScreenToWorldPoint(Vector2 screenPoint)
        {
            var yaw = ((screenPoint.X / xResolution - 0.5f) * HorizontalFov) * MathConstants.Deg2Rad;
            var pitch = - ((screenPoint.Y / yReslution - 0.5f) * VerticalFov) * MathConstants.Deg2Rad;
            var rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
            var transformedDirection = Vector3.Transform(transform.Forward, rotation);
            return transform.position + transformedDirection;
        }
    }
}