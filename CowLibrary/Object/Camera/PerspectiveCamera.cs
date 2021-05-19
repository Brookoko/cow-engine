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
                tan = (float) Math.Tan(Const.Deg2Rad * value / 2);
            }
        }
        
        private float _fov;
        private float tan;
        
        /// <summary> Creates camera-space ray from camera through screen space point </summary>
        /// <param name="screenPoint"> Scree space coordinates </param>
        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            var x = (2 * screenPoint.X / width - 1) * tan;
            var y = (1 - 2 * screenPoint.Y / height) / aspectRatio * tan;
            var dir = new Vector3(x, y, -nearPlane);
            dir = transform.localToWorldMatrix.MultiplyVector(dir).Normalize();
            return new Ray(transform.position, dir);
        }
    }
}