namespace CowLibrary
{
    public class RenderableObject : SceneObject
    {
        public IMesh Mesh { get; }
        public IMaterial Material { get; }

        public RenderableObject(IMesh mesh, IMaterial material)
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
