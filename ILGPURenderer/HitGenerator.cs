namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary;
using CowLibrary.Models;
using CowRenderer;
using Data;
using ILGPU;
using ILGPU.Runtime;

public interface IHitGenerator
{
    ArrayView3D<RayHit, Stride3D.DenseXY> GenerateHits(SceneModel scene, ArrayView3D<Ray, Stride3D.DenseXY> rays);
}

public class HitGenerator : IHitGenerator
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    private Action<Index3D,
        ArrayView3D<Ray, Stride3D.DenseXY>,
        ArrayView3D<RayHit, Stride3D.DenseXY>,
        MeshModel, LocalRaycaster> hitAction;

    [PostConstruct]
    public void Initialize()
    {
        hitAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<Ray, Stride3D.DenseXY>,
            ArrayView3D<RayHit, Stride3D.DenseXY>,
            MeshModel,
            LocalRaycaster
        >(GenerateHits);
    }

    public ArrayView3D<RayHit, Stride3D.DenseXY> GenerateHits(SceneModel scene, ArrayView3D<Ray, Stride3D.DenseXY> rays)
    {
        var buffer = GpuKernel.Accelerator.Allocate3DDenseXY<RayHit>(rays.Extent);
        var raycaster = new LocalRaycaster(scene.mesh.Count);
        hitAction(buffer.IntExtent, rays, buffer.View, scene.mesh, raycaster);
        return buffer.View;
    }

    private static void GenerateHits(Index3D index,
        ArrayView3D<Ray, Stride3D.DenseXY> rays,
        ArrayView3D<RayHit, Stride3D.DenseXY> hits,
        MeshModel mesh, LocalRaycaster raycaster)
    {
        hits[index] = raycaster.Raycast(in mesh, in rays[index]);
    }
}
