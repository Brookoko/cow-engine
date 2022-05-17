namespace ILGPURenderer;

using System;
using System.Numerics;
using Cowject;
using CowLibrary;
using CowLibrary.Mathematics.Sampler;
using CowLibrary.Models;
using CowRenderer;
using Data;
using ILGPU;
using ILGPU.Runtime;

public interface IRenderKernel
{
    Color[,] Render(in SceneView sceneView, Camera camera);
}

public class RenderKernel : IRenderKernel
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    [Inject]
    public ILocalSamplerProvider LocalSamplerProvider { get; set; }

    [Inject]
    public RenderConfig RenderConfig { get; set; }

    private Action<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            OrthographicCameraModel,
            Matrix4x4,
            int>
        orthographicAction;

    private Action<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            PerspectiveCameraModel,
            Matrix4x4,
            int>
        perspectiveAction;

    private Action<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            RealisticCameraModel,
            Matrix4x4,
            int>
        realisticAction;

    [PostConstruct]
    public void Initialize()
    {
        orthographicAction = LoadActionForOrthographic();
        perspectiveAction = LoadActionForPerspective();
        realisticAction = LoadActionForRealistic();
    }

    private Action<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            OrthographicCameraModel,
            Matrix4x4,
            int>
        LoadActionForOrthographic()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            OrthographicCameraModel,
            Matrix4x4,
            int
        >(Render);
    }

    private Action<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            PerspectiveCameraModel,
            Matrix4x4,
            int>
        LoadActionForPerspective()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            PerspectiveCameraModel,
            Matrix4x4,
            int
        >(Render);
    }

    private Action<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            RealisticCameraModel,
            Matrix4x4,
            int>
        LoadActionForRealistic()
    {
        return GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            RealisticCameraModel,
            Matrix4x4,
            int
        >(Render);
    }

    public Color[,] Render(in SceneView sceneView, Camera camera)
    {
        var size = new LongIndex2D(camera.Height, camera.Width);
        var buffer = GpuKernel.Accelerator.Allocate2DDenseX<Color>(size);
        GeneratePrimaryRaysForCamera(buffer, in sceneView, camera);
        return buffer.GetAsArray2D();
    }

    private void GeneratePrimaryRaysForCamera(MemoryBuffer2D<Color, Stride2D.DenseX> buffer, in SceneView sceneView,
        Camera camera)
    {
        var sampler = LocalSamplerProvider.GetSampler();
        var raycaster = new LocalRaycaster();
        var integrator = new LocalIntegrator();
        var matrix = camera.Transform.LocalToWorldMatrix;
        var samples = RenderConfig.numberOfRayPerPixel;
        var model = camera.Model;
        switch (model)
        {
            case OrthographicCameraModel orthographicCameraModel:
                orthographicAction(buffer.IntExtent, buffer.View, sceneView, sampler, raycaster, integrator, orthographicCameraModel, matrix, samples);
                break;
            case PerspectiveCameraModel perspectiveCameraModel:
                perspectiveAction(buffer.IntExtent, buffer.View, sceneView, sampler, raycaster, integrator, perspectiveCameraModel, matrix, samples);
                break;
            case RealisticCameraModel realisticCameraModel:
                realisticAction(buffer.IntExtent, buffer.View, sceneView, sampler, raycaster, integrator, realisticCameraModel, matrix, samples);
                break;
        }
    }

    private static void Render<TCamera>(
        Index2D index,
        ArrayView2D<Color, Stride2D.DenseX> colors,
        SceneView sceneView,
        LocalSampler sampler,
        LocalRaycaster raycaster,
        LocalIntegrator integrator,
        TCamera camera,
        Matrix4x4 cameraLocalToWorld,
        int samples)
        where TCamera : struct, ICameraModel
    {
        var point = new Vector2(index.Y + 0.5f, index.X + 0.5f);
        var color = Color.Black;
        for (var i = 0; i < samples; i++)
        {
            var sample = sampler.CreateSample();
            var ray = camera.ScreenPointToRay(in point, in cameraLocalToWorld, in sample);
            var hitRay = raycaster.Raycast(in sceneView.mesh, in ray);
            color += integrator.GetColor(in hitRay);
        }
        colors[index] = color / samples;
    }
}
