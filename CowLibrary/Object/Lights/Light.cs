namespace CowLibrary.Lights
{
    using System.Numerics;

    public abstract class Light : SceneObject
    {
        public abstract ShadingInfo GetShadingInfo(Vector3 point);

        public abstract Color Sample(Vector3 wi);
    }
    
    public class ShadingInfo
    {
        public Vector3 direction;
        public float distance;
        public Color color;
    }
}