namespace CowRenderer.Integration.Impl
{
    using System;
    using System.Numerics;
    using CowLibrary;

    public class NormalsIntegrator : IIntegrator
    {
        private readonly Color negativeColor = new Color(255, 255, 255);

        private readonly (Color, Color) lerpingColors = (new Color(255,84,82), new Color(115,2,0));  
        
        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return negativeColor;
            }
            
            var camera = scene.camera;
            var cameraDirection = camera.transform.position - surfel.point;
            var angle = Math.Acos(Vector3.Dot(cameraDirection, surfel.normal)) * MathConstants.Rad2Deg;
            if (angle > 90)
            {
                return negativeColor;
            }

            return Color.LerpUnclamped(lerpingColors.Item1, lerpingColors.Item2, (float) angle / 90);
        }
    }
}