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
    public readonly ArrayView<MetalMaterial> metalMaterials;
    public readonly ArrayView<PlasticMaterial> plasticMaterials;
    public readonly ArrayView<BlendMaterial> blendMaterials;
    public readonly ArrayView<MicrofacetReflectionMaterial> microfacetMaterials;

    public MaterialView(
        ArrayView<DiffuseMaterial> diffuseMaterials,
        ArrayView<FresnelMaterial> fresnelMaterials,
        ArrayView<ReflectionMaterial> reflectionMaterials,
        ArrayView<TransmissionMaterial> transmissionMaterials,
        ArrayView<MetalMaterial> metalMaterials,
        ArrayView<PlasticMaterial> plasticMaterials,
        ArrayView<BlendMaterial> blendMaterials,
        ArrayView<MicrofacetReflectionMaterial> microfacetMaterials)
    {
        this.diffuseMaterials = diffuseMaterials;
        this.fresnelMaterials = fresnelMaterials;
        this.reflectionMaterials = reflectionMaterials;
        this.transmissionMaterials = transmissionMaterials;
        this.metalMaterials = metalMaterials;
        this.plasticMaterials = plasticMaterials;
        this.blendMaterials = blendMaterials;
        this.microfacetMaterials = microfacetMaterials;
    }

    public Color Sample(in int id, in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        var f = Color.Black;
        wi = Vector3.Zero;
        pdf = 0;
        Sample(in diffuseMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in fresnelMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in reflectionMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in transmissionMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in metalMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in plasticMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in blendMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        Sample(in microfacetMaterials, in id, in normal, in wo, in sample, ref wi, ref pdf, ref f);
        return f;
    }

    private void Sample<T>(in ArrayView<T> materials, in int id, in Vector3 normal, in Vector3 wo, in Vector2 sample,
        ref Vector3 wi, ref float pdf, ref Color f)
        where T : unmanaged, IMaterial
    {
        if (f != Color.Black)
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
