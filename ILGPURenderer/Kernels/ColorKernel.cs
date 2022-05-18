namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary;
using Data;
using ILGPU;
using ILGPU.Runtime;

public interface IColorKernel
{
    ArrayView3D<Color, Stride3D.DenseXY> GenerateColors(in SceneView sceneView,
        in ArrayView3D<RayHit, Stride3D.DenseXY> hits);
}

public class ColorKernel : IColorKernel
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    private Action<Index3D,
        ArrayView3D<RayHit, Stride3D.DenseXY>,
        ArrayView3D<Color, Stride3D.DenseXY>,
        SceneView,
        LocalIntegrator> hitAction;

    [PostConstruct]
    public void Initialize()
    {
        hitAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<RayHit, Stride3D.DenseXY>,
            ArrayView3D<Color, Stride3D.DenseXY>,
            SceneView,
            LocalIntegrator
        >(GenerateColors);
    }

    public ArrayView3D<Color, Stride3D.DenseXY> GenerateColors(in SceneView sceneView,
        in ArrayView3D<RayHit, Stride3D.DenseXY> hits)
    {
        var colorBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<Color>(hits.Extent);
        var integrator = new LocalIntegrator();
        hitAction(colorBuffer.IntExtent, hits, colorBuffer.View, sceneView, integrator);
        return colorBuffer;
    }

    private static void GenerateColors(Index3D index,
        ArrayView3D<RayHit, Stride3D.DenseXY> hits,
        ArrayView3D<Color, Stride3D.DenseXY> colors,
        SceneView sceneView,
        LocalIntegrator integrator)
    {
        // colors[index] = integrator.GetColor(in sceneView, in hits[index]);
    }
}
