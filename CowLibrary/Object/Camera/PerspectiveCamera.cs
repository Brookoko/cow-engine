namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class PerspectiveCamera : Camera
    {
        /// <summary> Horizontal field of view </summary>
        public float Fov
        {
            get => fov;
            set
            {
                fov = value;
                tan = (float)Math.Tan(Const.Deg2Rad * value / 2);
            }
        }

        private float fov;
        private float tan;

        /// <summary> Creates camera-space ray from camera through screen space point </summary>
        /// <param name="screenPoint"> Scree space coordinates </param>
        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            var x = (2 * (screenPoint.X + 0.5f) / width - 1) * tan;
            var y = (1 - 2 * (screenPoint.Y + 0.5f) / height) / AspectRatio * tan;
            var dir = new Vector3(x, y, -nearPlane);
            dir = Transform.LocalToWorldMatrix.MultiplyVector(dir).Normalize();
            return new Ray(Transform.Position, dir);
        }

        public override Ray[] Sample(Vector2 screenPoint, int samples)
        {
            var rays = new Ray[samples];
            for (var i = 0; i < samples; i++)
            {
                var sample = Mathf.CreateSample() - 0.5f * Vector2.One;
                rays[i] = ScreenPointToRay(screenPoint + sample);
            }
            return rays;
        }
    }
}
