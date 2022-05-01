namespace CowLibrary
{
    public class RenderableObject : SceneObject
    {
        public Mesh Mesh { get; }
        public Material Material { get; }

        public RenderableObject(Mesh mesh, Material material)
        {
            Mesh = mesh;
            Material = material;
        }

        public void Prepare()
        {
            Mesh.Apply(Transform.LocalToWorldMatrix);
        }
    }
}
