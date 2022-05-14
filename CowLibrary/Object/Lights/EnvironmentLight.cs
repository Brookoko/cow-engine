namespace CowLibrary.Lights
{
    using System;
    using System.Numerics;

    public class EnvironmentLight : Light
    {
        private readonly Color color;
        private readonly float intensity;

        public EnvironmentLight(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }

        public override ShadingInfo GetShadingInfo(in Surfel surfel)
        {
            return new ShadingInfo()
            {
                direction = Mathf.CosineSampleHemisphere(surfel.normal, RandomF.CreateSample()),
                distance = float.PositiveInfinity,
                color = color * intensity
            };
        }

        public override Color Sample(in Vector3 wi)
        {
            return color * intensity;
        }
    }
}
