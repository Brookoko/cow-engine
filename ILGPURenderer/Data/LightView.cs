namespace ILGPURenderer.Data;

using System.Numerics;
using CowLibrary.Lights.Models;
using ILGPU;

public readonly struct LightView
{
    public readonly ArrayView<DirectionalLightModel> directionalLights;
    public readonly ArrayView<PointLightModel> pointLights;
    public readonly ArrayView<EnvironmentLightModel> environmentLights;
    public readonly ArrayView<Matrix4x4> matrices;

    public LightView(
        ArrayView<DirectionalLightModel> directionalLights,
        ArrayView<PointLightModel> pointLights,
        ArrayView<EnvironmentLightModel> environmentLights,
        ArrayView<Matrix4x4> matrices)
    {
        this.directionalLights = directionalLights;
        this.pointLights = pointLights;
        this.environmentLights = environmentLights;
        this.matrices = matrices;
    }

    public Matrix4x4 GetMatrix(in int id)
    {
        return matrices[id];
    }
}
