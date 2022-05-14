namespace CowLibrary.Models
{
    using System.Numerics;

    public interface ICameraModel
    {
        public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix);

        public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, int samples);
    }
}
