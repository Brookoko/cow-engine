namespace ILGPURenderer.Data;

using CowLibrary;

public readonly struct MaterialView
{
    public readonly DiffuseMaterial[] diffuseMaterials;
    public readonly FresnelMaterial[] fresnelMaterials;
    public readonly ReflectionMaterial[] reflectionMaterials;
    public readonly TransmissionMaterial[] transmissionMaterials;

    public MaterialView(DiffuseMaterial[] diffuseMaterials, FresnelMaterial[] fresnelMaterials,
        ReflectionMaterial[] reflectionMaterials, TransmissionMaterial[] transmissionMaterials)
    {
        this.diffuseMaterials = diffuseMaterials;
        this.fresnelMaterials = fresnelMaterials;
        this.reflectionMaterials = reflectionMaterials;
        this.transmissionMaterials = transmissionMaterials;
    }
}
