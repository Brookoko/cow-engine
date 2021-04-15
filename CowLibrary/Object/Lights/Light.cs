namespace CowLibrary.Lights
{
    using System.Numerics;

    public abstract class Light
    {
        public abstract Color Color { get; }
        
        public readonly Transform transform = new Transform();
        
        public abstract Vector3 GetDirection(Vector3 point);
        
        public abstract float GetIntensity(Vector3 point);
        
        public void Apply(Matrix4x4 matrix)
        {
            transform.localToWorldMatrix *= matrix;
        }
    }
}