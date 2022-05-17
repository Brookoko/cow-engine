namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public interface IColorKernel
{
    ArrayView3D<Color, Stride3D.DenseXY> GenerateColors(ArrayView3D<RayHit, Stride3D.DenseXY> hits);
}

public class ColorKernel : IColorKernel
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    private Action<Index3D,
        ArrayView3D<RayHit, Stride3D.DenseXY>,
        ArrayView3D<Color, Stride3D.DenseXY>,
        LocalIntegrator> hitAction;

    [PostConstruct]
    public void Initialize()
    {
        hitAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<RayHit, Stride3D.DenseXY>,
            ArrayView3D<Color, Stride3D.DenseXY>,
            LocalIntegrator
        >(GenerateColors);
    }

    public ArrayView3D<Color, Stride3D.DenseXY> GenerateColors(ArrayView3D<RayHit, Stride3D.DenseXY> hits)
    {
        var colorBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<Color>(hits.Extent);
        var integrator = new LocalIntegrator();
        hitAction(colorBuffer.IntExtent, hits, colorBuffer.View, integrator);
        return colorBuffer;
    }

    private static void GenerateColors(Index3D index,
        ArrayView3D<RayHit, Stride3D.DenseXY> hits,
        ArrayView3D<Color, Stride3D.DenseXY> colors,
        LocalIntegrator integrator)
    {
        colors[index] = integrator.GetColor(in hits[index]);
    }
}
