namespace CowLibrary.Models
{
    using System.Numerics;
    using Mathematics.Sampler;

    public interface ICameraModel : ICameraModelLocal
    {
        public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, ISampler sampler);

        public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, ISampler sampler, int samples);
    }

    public interface ICameraModelLocal
    {
        public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in LocalSampler sampler);

        public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in LocalSampler sampler, int samples);
    }
}
