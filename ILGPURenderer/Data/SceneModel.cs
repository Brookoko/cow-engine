namespace ILGPURenderer.Data;

public readonly struct SceneModel
{
    public readonly MeshModel mesh;

    public readonly MaterialModel material;

    public SceneModel(MeshModel mesh, MaterialModel material)
    {
        this.mesh = mesh;
        this.material = material;
    }
}
