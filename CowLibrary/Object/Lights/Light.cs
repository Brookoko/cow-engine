namespace CowLibrary.Lights
{
    using System.Numerics;

    public abstract class Light
    {
        public readonly Transform transform = new Transform();
        
        public abstract ShadingInfo GetShadingInfo(Vector3 point);
    }
    
    public class ShadingInfo
    {
        public Vector3 direction;
        public float distance;
        public Color color;
    }
}