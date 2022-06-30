namespace CowLibrary.Lights
{
    using Mathematics.Sampler;
    using Models;

    public class EnvironmentLight : Light
    {
        public override ILightModel Model { get; }

        public EnvironmentLight(Color color, float intensity, int id, ISampler sampler) : base(sampler)
        {
            Model = new EnvironmentLightModel(color * intensity, id);
        }
    }
}
