namespace CowLibrary.Lights
{
    using System.Numerics;
    using Mathematics.Sampler;

    public class EnvironmentLight : Light
    {
        private readonly Color color;
        private readonly float intensity;
        private readonly ISampler sampler;

        public EnvironmentLight(Color color, float intensity, ISampler sampler)
        {
            this.color = color;
            this.intensity = intensity;
            this.sampler = sampler;
        }

        public override ShadingInfo GetShadingInfo(in Surfel surfel)
        {
            return new ShadingInfo()
            {
                direction = Mathf.CosineSampleHemisphere(surfel.normal, sampler.CreateSample()),
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
