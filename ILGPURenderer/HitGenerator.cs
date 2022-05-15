namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary;
using Data;
using ILGPU;
using ILGPU.Runtime;

public interface IHitGenerator
{
    RayHit[,,] GenerateHits(SceneModel scene, Ray[,,] rays);
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

    public RayHit[,,] GenerateHits(SceneModel scene, Ray[,,] rays)
    {
        var size = new LongIndex3D(rays.GetLength(0), rays.GetLength(1), rays.GetLength(2));
        var rayBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<Ray>(size);
        rayBuffer.CopyFromCPU(rays);
        var rayHitBuffer = GpuKernel.Accelerator.Allocate3DDenseXY<RayHit>(size);
        var raycaster = new LocalRaycaster(scene.mesh.Count);
        hitAction(rayHitBuffer.IntExtent, rayBuffer.View, rayHitBuffer.View, scene.mesh, raycaster);
        return rayHitBuffer.GetAsArray3D();
    }

    private static void GenerateHits(Index3D index,
        ArrayView3D<Ray, Stride3D.DenseXY> rays,
        ArrayView3D<RayHit, Stride3D.DenseXY> hits,
        MeshModel mesh, LocalRaycaster raycaster)
    {
        hits[index] = raycaster.Raycast(in mesh, in rays[index]);
    }
}
