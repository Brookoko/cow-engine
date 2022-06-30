namespace ILGPURenderer;

using System;
using System.Numerics;
using System.Reflection;
using Converters;
using Cowject;
using CowLibrary;
using CowLibrary.Mathematics.Sampler;
using CowLibrary.Models;
using CowRenderer;
using Data;
using ILGPU;
using ILGPU.Backends;
using ILGPU.Backends.EntryPoints;
using ILGPU.Runtime;

public interface IRenderKernel
{
    void Prepare(Scene scene);

    Color[,] Render(Camera camera);
}

public class RenderKernel : IRenderKernel
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    [Inject]
    public ILocalSamplerProvider LocalSamplerProvider { get; set; }

    [Inject]
    public ISceneConverter SceneConverter { get; set; }

    [Inject]
    public RenderConfig RenderConfig { get; set; }

    private SceneView sceneView;

    private Action<
            AcceleratorStream,
            KernelConfig,
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
            AcceleratorStream,
            KernelConfig,
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
            AcceleratorStream,
            KernelConfig,
            ArrayView2D<Color, Stride2D.DenseX>,
            SceneView,
            LocalSampler,
            LocalRaycaster,
            LocalIntegrator,
            RealisticCameraModel,
            Matrix4x4,
            RenderData>
        realisticAction;

    public void Prepare(Scene scene)
    {
        LoadScene(scene);
        LoadKernel(scene.MainCamera.Model);
    }

    private void LoadScene(Scene scene)
    {
        sceneView = SceneConverter.Convert(scene);
    }

    private void LoadKernel(ICameraModel model)
    {
        switch (model)
        {
            case OrthographicCameraModel:
                orthographicAction = LoadKernel<OrthographicCameraModel>().CreateLauncherDelegate<Action<
                    AcceleratorStream,
                    KernelConfig,
                    ArrayView2D<Color, Stride2D.DenseX>,
                    SceneView,
                    LocalSampler,
                    LocalRaycaster,
                    LocalIntegrator,
                    OrthographicCameraModel,
                    Matrix4x4,
                    RenderData>>();
                break;
            case PerspectiveCameraModel:
                perspectiveAction = LoadKernel<PerspectiveCameraModel>().CreateLauncherDelegate<Action<
                    AcceleratorStream,
                    KernelConfig,
                    ArrayView2D<Color, Stride2D.DenseX>,
                    SceneView,
                    LocalSampler,
                    LocalRaycaster,
                    LocalIntegrator,
                    PerspectiveCameraModel,
                    Matrix4x4,
                    RenderData>>();
                break;
            case RealisticCameraModel:
                realisticAction = LoadKernel<RealisticCameraModel>().CreateLauncherDelegate<Action<
                    AcceleratorStream,
                    KernelConfig,
                    ArrayView2D<Color, Stride2D.DenseX>,
                    SceneView,
                    LocalSampler,
                    LocalRaycaster,
                    LocalIntegrator,
                    RealisticCameraModel,
                    Matrix4x4,
                    RenderData>>();
                break;
        }
    }

    private Kernel LoadKernel<T>() where T : ICameraModel
    {
        var backend = GpuKernel.Accelerator.GetBackend();
        var method = typeof(RenderKernel).GetMethod(nameof(Render), BindingFlags.NonPublic | BindingFlags.Static);
        var genericMethod = method.MakeGenericMethod(typeof(T));
        var entryPointDesc = EntryPointDescription.FromExplicitlyGroupedKernel(genericMethod);
        var specification = new KernelSpecialization(GpuKernel.Accelerator.MaxNumThreadsPerGroup,
            GpuKernel.Accelerator.MaxNumThreadsPerMultiprocessor);
        var compiledKernel = backend.Compile(entryPointDesc, specification);
        return GpuKernel.Accelerator.LoadKernel(compiledKernel);
    }

    public Color[,] Render(Camera camera)
    {
        var size = new LongIndex2D(camera.Height, camera.Width);
        var buffer = GpuKernel.Accelerator.Allocate2DDenseX<Color>(size);
        Render(buffer, camera);
        return buffer.GetAsArray2D();
    }

    private void Render(MemoryBuffer2D<Color, Stride2D.DenseX> buffer, Camera camera)
    {
        var sampler = LocalSamplerProvider.GetSampler();
        var raycaster = new LocalRaycaster();
        var integrator = new LocalIntegrator(sampler, raycaster);
        var matrix = camera.Transform.LocalToWorldMatrix;
        var renderData = new RenderData(RenderConfig);
        var model = camera.Model;
        var groupSize = GpuKernel.Accelerator.WarpSize * 2;
        var config = new KernelConfig(((int)buffer.Length + groupSize - 1) / groupSize, groupSize);
        switch (model)
        {
            case OrthographicCameraModel orthographicCameraModel:
                orthographicAction(GpuKernel.Accelerator.DefaultStream, config, buffer.View, sceneView,
                    sampler, raycaster, integrator,
                    orthographicCameraModel, matrix, renderData);
                break;
            case PerspectiveCameraModel perspectiveCameraModel:
                perspectiveAction(GpuKernel.Accelerator.DefaultStream, config, buffer.View, sceneView,
                    sampler, raycaster, integrator,
                    perspectiveCameraModel, matrix, renderData);
                break;
            case RealisticCameraModel realisticCameraModel:
                realisticAction(GpuKernel.Accelerator.DefaultStream, config, buffer.View, sceneView,
                    sampler, raycaster, integrator,
                    realisticCameraModel, matrix, renderData);
                break;
        }
    }

    private static void Render<TCamera>(
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
        var index = Grid.GlobalIndex.X;
        var w = colors.IntExtent.X;
        var x = index / w;
        var y = index % w;

        var point = new Vector2(x + 0.5f, y + 0.5f);
        var samples = renderData.numberOfRayPerPixelDimension;
        var step = 1f / (samples + 1);
        var centerIndex = (samples - 1) / 2f;

        var color = Color.Black;
        for (var i = 0; i < samples; i++)
        {
            for (var j = 0; j < samples; j++)
            {
                var samplePoint = point + new Vector2((i - centerIndex) * step, (j - centerIndex) * step);
                var sample = sampler.CreateSample();
                var ray = camera.ScreenPointToRay(in samplePoint, in cameraLocalToWorld, in sample);
                var raycast = raycaster.Raycast(in sceneView.mesh, in ray);
                color += integrator.GetColor(in sceneView, in renderData, in raycast);
            }
        }
        colors[y, x] = color / (samples * samples);
    }
}
