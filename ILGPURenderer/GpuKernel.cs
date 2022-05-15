namespace ILGPURenderer
{
    using System;
    using CowRenderer;
    using ILGPU;
    using ILGPU.Runtime;
    using ILGPU.Runtime.CPU;
    using ILGPU.Runtime.Cuda;

    public class GpuKernel : IDisposable
    {
        public Context Context { get; }
        public Accelerator Accelerator { get; }

        public GpuKernel(KernelMode mode)
        {
            Context = Context.Create(b => b.Cuda().EnableAlgorithms().CPU(new CPUDevice(4, 4, 1)).Assertions());
            Accelerator = mode.GetAccelerator(Context);
        }

        public void Dispose()
        {
            Accelerator.Dispose();
            Context.Dispose();
        }
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
