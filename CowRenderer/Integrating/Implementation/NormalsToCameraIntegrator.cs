namespace CowRenderer.Integration.Impl
{
    using System.Numerics;
    using CowLibrary;

    public class NormalsToCameraIntegrator : IIntegrator
    {
        private readonly Color negativeColor = new Color(255, 255, 255);

        private readonly Color errorColor = new Color(0, 0, 0);

        private readonly (Color, Color) lerpingColors = (new Color(255,0,0), new Color(0,0,250));  
        
        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return negativeColor;
            }
            
            var camera = scene.camera;
            var surfelToCameraDirection = /*camera.transform.position*/ Vector3.Zero - surfel.point;
            var angle = surfelToCameraDirection.AngleTo(surfel.normal);
            // if (angle > 90)
            // {
            //     return errorColor;
            // }
            
            return Color.LerpUnclamped(lerpingColors.Item1, lerpingColors.Item2, (float) angle / 180);
        }
    }
}