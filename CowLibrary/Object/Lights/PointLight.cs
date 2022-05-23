namespace CowLibrary.Lights
{
    using Mathematics.Sampler;
    using Models;

    public class PointLight : Light
    {
        public override ILightModel Model { get; }

        public PointLight(Color color, float intensity, ISampler sampler) : base(sampler)
        {
            Model = new PointLightModel(color * intensity);
        }
    }
}
