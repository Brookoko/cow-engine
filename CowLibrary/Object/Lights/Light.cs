namespace CowLibrary.Lights
{
    using System.Numerics;

    public abstract class Light : SceneObject
    {
        public abstract ShadingInfo GetShadingInfo(in RayHit rayHit);

        public abstract Color Sample(in Vector3 wi);
    }

    public class ShadingInfo
    {
        public Vector3 direction;
        public float distance;
        public Color color;
    }
}
