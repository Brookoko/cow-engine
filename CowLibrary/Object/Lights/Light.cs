namespace CowLibrary.Lights
{
    using System.Numerics;
    using Mathematics.Sampler;
    using Models;

    public abstract class Light : SceneObject
    {
        public abstract ILightModel Model { get; }

        private readonly ISampler sampler;

        protected Light(ISampler sampler)
        {
            this.sampler = sampler;
        }

        public ShadingInfo GetShadingInfo(in RayHit rayHit)
        {
            return Model.GetShadingInfo(in rayHit, Transform.LocalToWorldMatrix, sampler.CreateSample());
        }

        public Color Sample(in Vector3 wi)
        {
            return Model.Sample(in wi, sampler.CreateSample());
        }
    }
}
