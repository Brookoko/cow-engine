namespace CowLibrary
{
    public class RenderableObject : SceneObject
    {
        public IMesh Mesh { get; }
        public Material Material { get; }

        public RenderableObject(IMesh mesh, Material material)
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
