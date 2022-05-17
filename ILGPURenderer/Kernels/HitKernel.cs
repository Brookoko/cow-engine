namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary;
using Data;
using ILGPU;
using ILGPU.Runtime;

public interface IHitKernel
{
    ArrayView3D<RayHit, Stride3D.DenseXY> GenerateHits(SceneView scene, ArrayView3D<Ray, Stride3D.DenseXY> rays);
}

public class HitKernel : IHitKernel
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    private Action<Index3D,
        ArrayView3D<Ray, Stride3D.DenseXY>,
        ArrayView3D<RayHit, Stride3D.DenseXY>,
        MeshView, LocalRaycaster> hitAction;

    [PostConstruct]
    public void Initialize()
    {
        hitAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<Ray, Stride3D.DenseXY>,
            ArrayView3D<RayHit, Stride3D.DenseXY>,
            MeshView,
            LocalRaycaster
        >(GenerateHits);
    }

    public ArrayView3D<RayHit, Stride3D.DenseXY> GenerateHits(SceneView scene, ArrayView3D<Ray, Stride3D.DenseXY> rays)
    {
        var rayHitBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<RayHit>(rays.Extent);
        var raycaster = new LocalRaycaster(scene.mesh.count);
        hitAction(rayHitBuffer.IntExtent, rays, rayHitBuffer.View, scene.mesh, raycaster);
        return rayHitBuffer;
    }

    private static void GenerateHits(Index3D index,
        ArrayView3D<Ray, Stride3D.DenseXY> rays,
        ArrayView3D<RayHit, Stride3D.DenseXY> hits,
        MeshView mesh, LocalRaycaster raycaster)
    {
        hits[index] = raycaster.Raycast(in mesh, in rays[index]);
    }
}
