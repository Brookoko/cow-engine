namespace CowLibrary.Lights
{
    using System.Numerics;

    public class DirectionalLight : Light
    {
        public override Color Color { get; }

        private readonly float intensity;
        
        public DirectionalLight(Color color, float intensity)
        {
            Color = color;
            this.intensity = intensity;
        }
        
        public override Vector3 GetDirection(Vector3 point)
        {
            return transform.forward;
        }
        
        public override float GetIntensity(Vector3 point)
        {
            return intensity;
        }
    }
}