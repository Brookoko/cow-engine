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

        public override ShadingInfo GetShadingInfo(in RayHit rayHit)
        {
            return new ShadingInfo()
            {
                direction = Mathf.CosineSampleHemisphere(rayHit.normal, sampler.CreateSample()),
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
