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
            Context = Context.Create(b => b
                .Cuda()
                .CPU(new CPUDevice(4, 4, 1))
                .EnableAlgorithms()
                .Math(MathMode.Fast32BitOnly)
                .AutoAssertions()
                .Inlining(InliningMode.Aggressive)
                .Optimize(OptimizationLevel.O2)
                .AutoIOOperations()
                .LibDevice());
            Accelerator = mode.GetAccelerator(Context);
        }

        public ArrayView<T> ConvertToView<T>(T[] array) where T : unmanaged
        {
            var buffer = Accelerator.Allocate1D<T>(array.Length);
            buffer.CopyFromCPU(array);
            return buffer.View;
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
