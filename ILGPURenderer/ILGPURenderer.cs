namespace ILGPURenderer
{
    using System;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Models;
    using CowLibrary.Mathematics.Sampler;
    using CowRenderer;
    using ILGPU;
    using ILGPU.Algorithms.Random;
    using ILGPU.Runtime;

    public class ILGPURenderer : IRenderer
    {
        [Inject]
        public RenderConfig RenderConfig { get; set; }

        public Image Render(Scene scene)
        {
            var camera = scene.MainCamera;
            var w = camera.Width;
            var h = camera.Height;
            var image = new Image(w, h);
            using var gpuKernel = new GpuKernel(KernelMode.Gpu);
            var random = new Random();
            using var rng = RNG.Create<XorShift64Star>(gpuKernel.Accelerator, random);
            var rngView = rng.GetView(gpuKernel.Accelerator.WarpSize);
            var sampler = new LocalSampler(rngView);
            var rays = GeneratePrimaryRays(gpuKernel, camera, in sampler);
            return image;
        }

        private Ray[,,] GeneratePrimaryRays(GpuKernel gpuKernel, Camera camera, in LocalSampler sampler)
        {
            var samples = RenderConfig.numberOfRayPerPixel;
            var size = new LongIndex3D(camera.Width, camera.Height, samples);
            using var buffer = gpuKernel.Accelerator.Allocate3DDenseXY<Ray>(size);
            var kernel = gpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
                Index3D,
                PerspectiveCameraModel,
                LocalSampler,
                ArrayView3D<Ray, Stride3D.DenseXY>,
                Matrix4x4
            >(GeneratedPrimaryRaysKernel);
            var model = camera.Model is PerspectiveCameraModel cameraModel ? cameraModel : default;
            kernel(buffer.Extent.ToIntIndex(), model, sampler, buffer.View, camera.Transform.LocalToWorldMatrix);
            return buffer.GetAsArray3D();
        }

        private static void GeneratedPrimaryRaysKernel<TCamera>(Index3D index, TCamera camera, LocalSampler sampler,
            ArrayView3D<Ray, Stride3D.DenseXY> rays,
            Matrix4x4 localToWorldMatrix)
            where TCamera : struct, ICameraModelLocal
        {
            var point = new Vector2(index.X + 0.5f, index.Y + 0.5f);
            rays[index] = camera.Sample(in point, in localToWorldMatrix, sampler, 1)[0];
        }
    }
}
