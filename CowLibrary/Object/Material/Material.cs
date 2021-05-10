namespace CowLibrary
{
    using System.Numerics;

    public abstract class Material
    {
        public readonly Color color;
        
        public Material(Color color)
        {
            this.color = color;
        }
        
        public abstract Color GetColor(Vector3 wo, Vector3 wi);
        
        public abstract float Sample(Surfel surfel, out Vector3 wi, out float pdf);
    }
}