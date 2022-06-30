namespace CowLibrary.Models
{
    using System.Numerics;

    public interface ICameraModel
    {
        public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in Vector2 sample);

        public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in Vector2[] samples);
    }
}
