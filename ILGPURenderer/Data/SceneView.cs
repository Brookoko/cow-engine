namespace ILGPURenderer.Data;

public readonly struct SceneView
{
    public readonly MeshView mesh;

    public readonly MaterialView material;

    public SceneView(MeshView mesh, MaterialView material)
    {
        this.mesh = mesh;
        this.material = material;
    }
}
