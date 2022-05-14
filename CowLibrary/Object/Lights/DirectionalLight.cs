namespace CowLibrary.Lights
{
    using System.Numerics;

    public class DirectionalLight : Light
    {
        private readonly Color color;
        private readonly float intensity;

        public DirectionalLight(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }

        public override ShadingInfo GetShadingInfo(in Surfel surfel)
        {
            return new ShadingInfo()
            {
                direction = Transform.Forward,
                distance = float.PositiveInfinity,
                color = color * intensity
            };
        }

        public override Color Sample(in Vector3 wi)
        {
            return Color.Black;
        }
    }
}
