namespace CowLibrary
{
    using System.Numerics;

    public class RenderableObject
    {
        public readonly Transform transform = new Transform();
        
        public readonly Mesh mesh;
        
        public readonly Material material;
        
        public RenderableObject(Mesh mesh, Material material)
        {
            this.mesh = mesh;
            this.material = material;
        }
        
        public void Apply(Matrix4x4 matrix)
        {
            mesh.Apply(transform.localToWorldMatrix * matrix);
        }
    }
}