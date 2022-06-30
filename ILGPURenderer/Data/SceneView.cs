namespace ILGPURenderer.Data;

using System.Numerics;
using CowLibrary;

public readonly struct SceneView
{
    public readonly MeshView mesh;

    public readonly MaterialView material;

    public readonly LightView light;

    public SceneView(MeshView mesh, MaterialView material, LightView light)
    {
        this.mesh = mesh;
        this.material = material;
        this.light = light;
    }
}
