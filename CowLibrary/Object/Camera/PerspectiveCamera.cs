namespace CowLibrary
{
    using System;
    using System.Numerics;

    public class PerspectiveCamera : Camera
    {
        /// <summary> Horizontal field of view </summary>
        public float fov
        {
            get => _fov;
            set
            {
                _fov = value;
                tan = (float) Math.Tan(MathConstants.Deg2Rad * value / 2);
            }
        }
        
        private float _fov;
        private float tan;
        
        /// <summary> Creates camera-space ray from camera through screen space point </summary>
        /// <param name="screenPoint"> Scree space coordinates </param>
        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            var x = (1 - 2 * (screenPoint.X + 0.5f) / width) * tan;
            var y = (2 * (screenPoint.Y + 0.5f) / height - 1) / AspectRatio * tan;
            var dir = new Vector3(x, y, -nearPlane);
            dir = transform.localToWorldMatrix.MultiplyVector(dir).Normalize();
            return new Ray(transform.position, dir);
        }
    }
}