namespace ILGPURenderer.Data;

using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public readonly struct PrimaryRays
{
    public readonly ArrayView3D<Ray, Stride3D.DenseXY> data;

    public PrimaryRays(in ArrayView3D<Ray, Stride3D.DenseXY> data)
    {
        this.data = data;
    }
}
