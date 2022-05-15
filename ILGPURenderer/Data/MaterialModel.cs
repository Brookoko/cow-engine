namespace ILGPURenderer.Data;

using CowLibrary;

public readonly struct MaterialModel
{
    public readonly DiffuseMaterial[] diffuseMaterials;
    public readonly FresnelMaterial[] fresnelMaterials;
    public readonly ReflectionMaterial[] reflectionMaterials;
    public readonly TransmissionMaterial[] transmissionMaterials;

    public MaterialModel(DiffuseMaterial[] diffuseMaterials, FresnelMaterial[] fresnelMaterials,
        ReflectionMaterial[] reflectionMaterials, TransmissionMaterial[] transmissionMaterials)
    {
        this.diffuseMaterials = diffuseMaterials;
        this.fresnelMaterials = fresnelMaterials;
        this.reflectionMaterials = reflectionMaterials;
        this.transmissionMaterials = transmissionMaterials;
    }
}
