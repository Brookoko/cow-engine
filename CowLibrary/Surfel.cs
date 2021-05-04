namespace CowLibrary
{
    using System.Numerics;

    public class Surfel
    {
        public float t;
        public Vector3 ray;
        public Vector3 point;
        public Vector3 normal;
        public Material material;
    }
}