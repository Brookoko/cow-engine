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
            RenderData>
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
            RenderData>
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
            RenderData>
        realisticAction;

    [PostConstruct]
    public void Initialize()
    {
        orthographicAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            OrthographicCameraModel,
            Matrix4x4,
            RenderData
        >(Render);
        perspectiveAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            PerspectiveCameraModel,
            Matrix4x4,
            RenderData
        >(Render);
        realisticAction = GpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
            Index2D,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            RealisticCameraModel,
            Matrix4x4,
            RenderData
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
        var integrator = new LocalIntegrator(sampler, raycaster);
        var matrix = camera.Transform.LocalToWorldMatrix;
        var renderData = new RenderData(RenderConfig);
        var model = camera.Model;
        switch (model)
        {
            case OrthographicCameraModel orthographicCameraModel:
                orthographicAction(buffer.IntExtent, buffer.View, sceneView, sampler, raycaster, integrator,
                    orthographicCameraModel, matrix, renderData);
                break;
            case PerspectiveCameraModel perspectiveCameraModel:
                perspectiveAction(buffer.IntExtent, buffer.View, sceneView, sampler, raycaster, integrator,
                    perspectiveCameraModel, matrix, renderData);
                break;
            case RealisticCameraModel realisticCameraModel:
                realisticAction(buffer.IntExtent, buffer.View, sceneView, sampler, raycaster, integrator,
                    realisticCameraModel, matrix, renderData);
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
        RenderData renderData)
        where TCamera : struct, ICameraModel
    {
        var point = new Vector2(index.Y + 0.5f, index.X + 0.5f);
        var color = Color.Black;
        for (var i = 0; i < renderData.numberOfRayPerPixel; i++)
        {
            var sample = sampler.CreateSample();
            var ray = camera.ScreenPointToRay(in point, in cameraLocalToWorld, in sample);
            var raycast = raycaster.Raycast(in sceneView.mesh, in ray);
            color += integrator.GetColor(in sceneView, in renderData, in raycast);
        }
        colors[index] = color / renderData.numberOfRayPerPixel;
    }
}
