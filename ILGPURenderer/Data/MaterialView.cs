namespace ILGPURenderer.Data;

using System.Numerics;
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

    public Color GetMaterialColor(in int id, in Vector3 wo, in Vector3 wi)
    {
        for (var i = 0; i < diffuseMaterials.Length; i++)
        {
            if (diffuseMaterials[i].Id == id)
            {
                return diffuseMaterials[i].GetColor(in wo, in wi);
            }
        }
        for (var i = 0; i < fresnelMaterials.Length; i++)
        {
            if (fresnelMaterials[i].Id == id)
            {
                return fresnelMaterials[i].GetColor(in wo, in wi);
            }
        }
        for (var i = 0; i < reflectionMaterials.Length; i++)
        {
            if (reflectionMaterials[i].Id == id)
            {
                return reflectionMaterials[i].GetColor(in wo, in wi);
            }
        }
        for (var i = 0; i < transmissionMaterials.Length; i++)
        {
            if (transmissionMaterials[i].Id == id)
            {
                return transmissionMaterials[i].GetColor(in wo, in wi);
            }
        }
        return Color.Black;
    }
}
