namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary.Mathematics.Sampler;
using ILGPU;
using ILGPU.Algorithms.Random;
using ILGPU.Runtime;

public interface ILocalSamplerProvider
{
    LocalSampler GetSampler();
}

public class LocalSamplerProvider : ILocalSamplerProvider, IDisposable
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    private MemoryBuffer1D<XorShift64Star, Stride1D.Dense> buffer;
    private LocalSampler sampler;

    [PostConstruct]
    public void Initialize()
    {
        var random = new Random();
        var randoms = new XorShift64Star[1];
        for (var i = 0; i < randoms.Length; i++)
        {
            randoms[i] = XorShift64Star.Create(random);
        }
        buffer = GpuKernel.Accelerator.Allocate1D<XorShift64Star>(randoms.Length);
        buffer.CopyFromCPU(randoms);
        sampler = new LocalSampler(buffer.View);
    }

    public LocalSampler GetSampler()
    {
        return sampler;
    }

    public void Dispose()
    {
        buffer?.Dispose();
    }
}
