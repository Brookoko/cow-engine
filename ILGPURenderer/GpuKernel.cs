namespace ILGPURenderer
{
    using System;
    using ILGPU;
    using ILGPU.Runtime;
    using ILGPU.Runtime.CPU;
    using ILGPU.Runtime.Cuda;

    public class GpuKernel : IDisposable
    {
        private readonly Context context;
        private readonly Accelerator accelerator;

        public GpuKernel(KernelMode mode)
        {
            context = Context.Create(b => b.Cuda().EnableAlgorithms().Assertions());
            accelerator = mode.GetAccelerator(context);
        }

        public void Dispose()
        {
            accelerator.Dispose();
            context.Dispose();
        }
    }

    public enum KernelMode
    {
        Gpu,
        Cpu
    }

    public static class KernelModeExtensions
    {
        public static Accelerator GetAccelerator(this KernelMode mode, Context context)
        {
            return mode switch
            {
                KernelMode.Gpu => context.CreateCudaAccelerator(0),
                KernelMode.Cpu => context.CreateCPUAccelerator(0),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, $"Unknown mode {mode}")
            };
        }
    }
}
