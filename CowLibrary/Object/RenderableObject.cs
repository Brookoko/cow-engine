namespace CowLibrary
{
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
        
        public void Prepare()
        {
            mesh.Apply(transform.localToWorldMatrix);
        }
    }
}