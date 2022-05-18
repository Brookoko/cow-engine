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

    public Color GetMaterialRawColor(in int id)
    {
        var color = Color.Black;
        GetMaterialRawColor(in diffuseMaterials, in id, ref color);
        GetMaterialRawColor(in fresnelMaterials, in id, ref color);
        GetMaterialRawColor(in reflectionMaterials, in id, ref color);
        GetMaterialRawColor(in transmissionMaterials, in id, ref color);
        return color;
    }

    private void GetMaterialRawColor<T>(in ArrayView<T> materials, in int id, ref Color color)
        where T : unmanaged, IMaterial
    {
        if (color != Color.Black)
        {
            return;
        }
        for (var i = 0; i < materials.Length; i++)
        {
            if (materials[i].Id == id)
            {
                color = materials[i].Color;
                return;
            }
        }
    }

    public Color GetMaterialColor(in int id, in Vector3 wo, in Vector3 wi)
    {
        var color = Color.Black;
        GetMaterialColor(in diffuseMaterials, in id, wo, wi, ref color);
        GetMaterialColor(in fresnelMaterials, in id, wo, wi, ref color);
        GetMaterialColor(in reflectionMaterials, in id, wo, wi, ref color);
        GetMaterialColor(in transmissionMaterials, in id, wo, wi, ref color);
        return color;
    }

    private void GetMaterialColor<T>(in ArrayView<T> materials, in int id, in Vector3 wo, in Vector3 wi,
        ref Color color)
        where T : unmanaged, IMaterial
    {
        if (color != Color.Black)
        {
            return;
        }
        for (var i = 0; i < materials.Length; i++)
        {
            if (materials[i].Id == id)
            {
                color = materials[i].GetColor(in wo, in wi);
                return;
            }
        }
    }

    public float Sample(in int id, in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var f = -1f;
        wi = Vector3.Zero;
        pdf = 0;
        Sample(in diffuseMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in fresnelMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in reflectionMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in transmissionMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        return f;
    }

    private void Sample<T>(in ArrayView<T> materials, in int id, in Vector3 normal, in Vector3 wo, in Vector2 sample,
        ref Vector3 wi, ref float pdf, ref float f)
        where T : unmanaged, IMaterial
    {
        if (f >= 0)
        {
            return;
        }
        for (var i = 0; i < materials.Length; i++)
        {
            if (materials[i].Id == id)
            {
                f = materials[i].Sample(in normal, in wo, in sample, out wi, out pdf);
                return;
            }
        }
    }
}
