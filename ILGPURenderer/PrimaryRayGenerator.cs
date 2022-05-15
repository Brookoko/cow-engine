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
    ArrayView3D<Ray, Stride3D.DenseXY> GeneratePrimaryRays(Camera camera);
}

public class PrimaryRayGenerator : IPrimaryRayGenerator
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    [Inject]
    public ILocalSamplerProvider LocalSamplerProvider { get; set; }

    [Inject]
    public RenderConfig RenderConfig { get; set; }

    private Action<Index3D, PrimaryRays, RayGenerationData, OrthographicCameraModel> orthographicAction;
    private Action<Index3D, PrimaryRays, RayGenerationData, PerspectiveCameraModel> perspectiveAction;
    private Action<Index3D, PrimaryRays, RayGenerationData, RealisticCameraModel> realisticAction;

    [PostConstruct]
    public void Initialize()
    {
        orthographicAction = LoadActionForOrthographic();
        perspectiveAction = LoadActionForPerspective();
        realisticAction = LoadActionForRealistic();
    }

    public ArrayView3D<Ray, Stride3D.DenseXY> GeneratePrimaryRays(Camera camera)
    {
        var samples = RenderConfig.numberOfRayPerPixel;
        var size = new LongIndex3D(camera.Width, camera.Height, samples);
        var buffer = GpuKernel.Accelerator.Allocate3DDenseXY<Ray>(size).View;
        GeneratePrimaryRaysForCamera(in buffer, camera);
        return buffer;
    }

    private void GeneratePrimaryRaysForCamera(in ArrayView3D<Ray, Stride3D.DenseXY> buffer, Camera camera)
    {
        var rays = new PrimaryRays(in buffer);
        var sampler = LocalSamplerProvider.GetSampler();
        var data = new RayGenerationData(camera.Transform.LocalToWorldMatrix, sampler);
        var model = camera.Model;
        switch (model)
        {
            case OrthographicCameraModel orthographicCameraModel:
                orthographicAction(buffer.IntExtent, rays, data, orthographicCameraModel);
                break;
            case PerspectiveCameraModel perspectiveCameraModel:
                perspectiveAction(buffer.IntExtent, rays, data, perspectiveCameraModel);
                break;
            case RealisticCameraModel realisticCameraModel:
                realisticAction(buffer.IntExtent, rays, data, realisticCameraModel);
                break;
        }
    }

    private Action<Index3D, PrimaryRays, RayGenerationData, OrthographicCameraModel> LoadActionForOrthographic()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            PrimaryRays,
            RayGenerationData,
            OrthographicCameraModel
        >(GeneratedPrimaryRaysKernel);
    }

    private Action<Index3D, PrimaryRays, RayGenerationData, PerspectiveCameraModel> LoadActionForPerspective()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            PrimaryRays,
            RayGenerationData,
            PerspectiveCameraModel
        >(GeneratedPrimaryRaysKernel);
    }

    private Action<Index3D, PrimaryRays, RayGenerationData, RealisticCameraModel> LoadActionForRealistic()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index3D,
            PrimaryRays,
            RayGenerationData,
            RealisticCameraModel
        >(GeneratedPrimaryRaysKernel);
    }

    private static void GeneratedPrimaryRaysKernel<TCamera>(Index3D index, PrimaryRays rays,
        RayGenerationData data, TCamera camera)
        where TCamera : struct, ICameraModel
    {
        var point = new Vector2(index.X + 0.5f, index.Y + 0.5f);
        var sample = data.sampler.CreateSample();
        rays.data[index] = camera.ScreenPointToRay(in point, in data.localToWorldMatrix, in sample);
    }
}
