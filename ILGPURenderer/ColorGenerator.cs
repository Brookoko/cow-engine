namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public interface IColorGenerator
{
    Color[,,] GenerateColors(RayHit[,,] hits);
}

public class ColorGenerator : IColorGenerator
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

    public Color[,,] GenerateColors(RayHit[,,] hits)
    {
        var size = new LongIndex3D(hits.GetLength(0), hits.GetLength(1), hits.GetLength(2));
        var hitBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<RayHit>(size);
        hitBuffer.CopyFromCPU(hits);
        var colorBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<Color>(size);
        var integrator = new LocalIntegrator();
        hitAction(colorBuffer.IntExtent, hitBuffer.View, colorBuffer.View, integrator);
        return colorBuffer.GetAsArray3D();
    }

    private static void GenerateColors(Index3D index,
        ArrayView3D<RayHit, Stride3D.DenseXY> hits,
        ArrayView3D<Color, Stride3D.DenseXY> colors,
        LocalIntegrator integrator)
    {
        colors[index] = integrator.GetColor(in hits[index]);
    }
}
