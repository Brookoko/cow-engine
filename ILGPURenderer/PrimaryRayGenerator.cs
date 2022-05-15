namespace ILGPURenderer;

using System;
using System.Numerics;
using Cowject;
using CowLibrary;
using CowLibrary.Models;
using CowRenderer;
using Data;
using ILGPU;
using ILGPU.Runtime;

public interface IPrimaryRayGenerator
{
    Ray[,,] GeneratePrimaryRays(Camera camera);
}

public class PrimaryRayGenerator : IPrimaryRayGenerator
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    [Inject]
    public ILocalSamplerProvider LocalSamplerProvider { get; set; }

    [Inject]
    public RenderConfig RenderConfig { get; set; }

    private Action<Index3D, ArrayView3D<Ray, Stride3D.DenseXY>, RayGenerationData, OrthographicCameraModel> orthographicAction;
    private Action<Index3D, ArrayView3D<Ray, Stride3D.DenseXY>, RayGenerationData, PerspectiveCameraModel> perspectiveAction;
    private Action<Index3D, ArrayView3D<Ray, Stride3D.DenseXY>, RayGenerationData, RealisticCameraModel> realisticAction;

    [PostConstruct]
    public void Initialize()
    {
        orthographicAction = LoadActionForOrthographic();
        perspectiveAction = LoadActionForPerspective();
        realisticAction = LoadActionForRealistic();
    }

    public Ray[,,] GeneratePrimaryRays(Camera camera)
    {
        var samples = RenderConfig.numberOfRayPerPixel;
        var size = new LongIndex3D(camera.Width, camera.Height, samples);
        using var buffer = GpuKernel.Accelerator.Allocate3DDenseXY<Ray>(size);
        GeneratePrimaryRaysForCamera(buffer, camera);
        return buffer.GetAsArray3D();
    }

    private void GeneratePrimaryRaysForCamera(in MemoryBuffer3D<Ray, Stride3D.DenseXY> buffer, Camera camera)
    {
        var sampler = LocalSamplerProvider.GetSampler();
        var data = new RayGenerationData(camera.Transform.LocalToWorldMatrix, sampler);
        var model = camera.Model;
        switch (model)
        {
            case OrthographicCameraModel orthographicCameraModel:
                orthographicAction(buffer.IntExtent, buffer.View, data, orthographicCameraModel);
                break;
            case PerspectiveCameraModel perspectiveCameraModel:
                perspectiveAction(buffer.IntExtent, buffer.View, data, perspectiveCameraModel);
                break;
            case RealisticCameraModel realisticCameraModel:
                realisticAction(buffer.IntExtent, buffer.View, data, realisticCameraModel);
                break;
        }
    }

    private Action<Index3D, ArrayView3D<Ray, Stride3D.DenseXY>, RayGenerationData, OrthographicCameraModel> LoadActionForOrthographic()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<Ray, Stride3D.DenseXY>,
            RayGenerationData,
            OrthographicCameraModel
        >(GeneratedPrimaryRaysKernel);
    }

    private Action<Index3D, ArrayView3D<Ray, Stride3D.DenseXY>, RayGenerationData, PerspectiveCameraModel> LoadActionForPerspective()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<Ray, Stride3D.DenseXY>,
            RayGenerationData,
            PerspectiveCameraModel
        >(GeneratedPrimaryRaysKernel);
    }

    private Action<Index3D, ArrayView3D<Ray, Stride3D.DenseXY>, RayGenerationData, RealisticCameraModel> LoadActionForRealistic()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            ArrayView3D<Ray, Stride3D.DenseXY>,
            RayGenerationData,
            RealisticCameraModel
        >(GeneratedPrimaryRaysKernel);
    }

    private static void GeneratedPrimaryRaysKernel<TCamera>(Index3D index, ArrayView3D<Ray, Stride3D.DenseXY> rays,
        RayGenerationData data, TCamera camera)
        where TCamera : struct, ICameraModel
    {
        var point = new Vector2(index.X + 0.5f, index.Y + 0.5f);
        var sample = data.sampler.CreateSample();
        rays[index] = camera.ScreenPointToRay(in point, in data.localToWorldMatrix, in sample);
    }
}
