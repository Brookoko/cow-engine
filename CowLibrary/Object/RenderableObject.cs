namespace CowLibrary
{
    public class RenderableObject
    {
        public Transform transform = new Transform();
        public Mesh mesh;
        public Material material;
        
        public RenderableObject()
        {
        }
        
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