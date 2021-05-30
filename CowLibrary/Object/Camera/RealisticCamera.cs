namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class RealisticCamera : Camera
    {
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
        private Lens lens;
        
        public RealisticCamera(int width, int height, float fov, Lens lens)
        {
            this.width = width;
            this.height = height;
            this.fov = fov;
            this.lens = lens;
        }
        
        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            return Sample(screenPoint, 1).First();
        }
        
        public override List<Ray> Sample(Vector2 screenPoint, int samples)
        {
            var point = ViewportPoint(screenPoint);
            var lensCenter = new Vector3(0, 0, lens.distance);
            var dir = (lensCenter - point).Normalize();
            var focusPoint = point + dir * (lens.distance + lens.focus);
            var rays = new List<Ray>();
            for (var i = 0; i < samples; i++)
            {
                var sample = Mathf.ConcentricSampleDisk(Mathf.CreateSample());
                var lensPoint = lensCenter + new Vector3(sample * lens.radius, 0);
                var direction = lensPoint - focusPoint;
                direction = transform.localToWorldMatrix.MultiplyVector(direction).Normalize();
                var ray = new Ray(transform.position, direction);
                rays.Add(ray);
            }
            return rays;
        }
        
        private Vector3 ViewportPoint(Vector2 screenPoint)
        {
            var x = (2 * (screenPoint.X + 0.5f) / width - 1) * tan;
            var y = (1 - 2 * (screenPoint.Y + 0.5f) / height) / aspectRatio * tan;
            return new Vector3(x, y, 0);
        }
    }
}