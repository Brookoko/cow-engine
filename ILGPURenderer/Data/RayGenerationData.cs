namespace ILGPURenderer.Data;

using System.Numerics;
using CowLibrary.Mathematics.Sampler;

public readonly struct RayGenerationData
{
    public readonly Matrix4x4 localToWorldMatrix;
    public readonly LocalSampler sampler;

    public RayGenerationData(Matrix4x4 localToWorldMatrix, LocalSampler sampler)
    {
        this.localToWorldMatrix = localToWorldMatrix;
        this.sampler = sampler;
    }
}
