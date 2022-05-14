namespace ILGPURenderer
{
    using CowLibrary;
    using CowRenderer;

    public class ILGPURenderer : IRenderer
    {
        public Image Render(Scene scene)
        {
            var camera = scene.MainCamera;
            var w = camera.width;
            var h = camera.height;
            var image = new Image(w, h);
            using var gpuKernel = new GpuKernel(KernelMode.Gpu);
            return image;
        }
    }
}
