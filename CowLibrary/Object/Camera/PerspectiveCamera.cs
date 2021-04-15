namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class PerspectiveCamera : Camera
    {
        public float fov
        {
            get => _fov;
            set
            {
                _fov = value;
                tan = (float) Math.Tan(MathConstants.Deg2Rad * value * 2);
            }
        }
        
        private float _fov;
        private float tan;
        
        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            var x = (2 * (screenPoint.X + 0.5f) / width - 1) * AspectRatio * tan;
            var y = (1 - 2 * (screenPoint.Y + 0.5f) / height) * tan;
            var dir = new Vector3(x, y, -nearPlane).Normalize();
            return new Ray(Vector3.Zero, dir);
        }
    }
}