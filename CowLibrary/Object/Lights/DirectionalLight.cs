namespace CowLibrary.Lights
{
    using Mathematics.Sampler;
    using Models;

    public class DirectionalLight : Light
    {
        public override ILightModel Model { get; }

        public DirectionalLight(Color color, float intensity, int id, ISampler sampler) : base(sampler)
        {
            Model = new DirectionalLightModel(color * intensity, id);
        }
    }
}
