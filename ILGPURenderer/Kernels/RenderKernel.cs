namespace ILGPURenderer;

using System;
using System.Numerics;
using System.Reflection;
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

    [PostConstruct]
    public void Initialize()
    {
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

    public Color[,] Render(in SceneView sceneView, Camera camera)
    {
        var size = new LongIndex2D(camera.Height, camera.Width);
        var buffer = GpuKernel.Accelerator.Allocate2DDenseX<Color>(size);
        Render(buffer, in sceneView, camera);
        return buffer.GetAsArray2D();
    }

    private void Render(MemoryBuffer2D<Color, Stride2D.DenseX> buffer, in SceneView sceneView,
        Camera camera)
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
        var x = index / 1080;
        var y = index % 1080;
        var point = new Vector2(x + 0.5f, y + 0.5f);
        var color = Color.Black;
        for (var i = 0; i < renderData.numberOfRayPerPixel; i++)
        {
            var sample = sampler.CreateSample();
            var ray = camera.ScreenPointToRay(in point, in cameraLocalToWorld, in sample);
            var raycast = raycaster.Raycast(in sceneView.mesh, in ray);
            color += integrator.GetColor(in sceneView, in renderData, in raycast);
        }
        colors[y, x] = color / renderData.numberOfRayPerPixel;
    }
}
