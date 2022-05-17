namespace ILGPURenderer.Data;

using CowLibrary;
using ILGPU;

public readonly struct MaterialView
{
    public readonly ArrayView<DiffuseMaterial> diffuseMaterials;
    public readonly ArrayView<FresnelMaterial> fresnelMaterials;
    public readonly ArrayView<ReflectionMaterial> reflectionMaterials;
    public readonly ArrayView<TransmissionMaterial> transmissionMaterials;

    public MaterialView(
        ArrayView<DiffuseMaterial> diffuseMaterials,
        ArrayView<FresnelMaterial> fresnelMaterials,
        ArrayView<ReflectionMaterial> reflectionMaterials,
        ArrayView<TransmissionMaterial> transmissionMaterials)
    {
        this.diffuseMaterials = diffuseMaterials;
        this.fresnelMaterials = fresnelMaterials;
        this.reflectionMaterials = reflectionMaterials;
        this.transmissionMaterials = transmissionMaterials;
    }
}
