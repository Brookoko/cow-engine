namespace ILGPURenderer
{
    using System;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Models;
    using CowRenderer;
    using ILGPU;
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
            var rays = GeneratePrimaryRays(gpuKernel, camera);
            foreach (var ray in rays)
            {
                Console.WriteLine($"{ray.origin} -> {ray.direction}");
            }
            return image;
        }

        private Ray[,,] GeneratePrimaryRays(GpuKernel gpuKernel, Camera camera)
        {
            var samples = RenderConfig.numberOfRayPerPixel;
            var size = new LongIndex3D(camera.Width, camera.Height, samples);
            using var buffer = gpuKernel.Accelerator.Allocate3DDenseXY<Ray>(size);
            var kernel = gpuKernel.Accelerator.LoadAutoGroupedStreamKernel<
                Index3D,
                ArrayView3D<Ray, Stride3D.DenseXY>,
                Matrix4x4,
                PerspectiveCameraModel>(GeneratedPrimaryRaysKernel);
            var model = camera.Model is PerspectiveCameraModel cameraModel ? cameraModel : default;
            kernel(buffer.Extent.ToIntIndex(), buffer.View, camera.Transform.LocalToWorldMatrix, model);
            return buffer.GetAsArray3D();
        }

        private static void GeneratedPrimaryRaysKernel<TCamera>(Index3D index, ArrayView3D<Ray, Stride3D.DenseXY> rays,
            Matrix4x4 localToWorldMatrix, TCamera camera)
            where TCamera : ICameraModel
        {
            var point = new Vector2(index.X + 0.5f, index.Y + 0.5f);
            rays[index] = camera.Sample(in point, in localToWorldMatrix, 1)[0];
        }
    }
}
