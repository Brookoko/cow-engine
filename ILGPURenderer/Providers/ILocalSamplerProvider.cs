namespace ILGPURenderer;

using System;
using Cowject;
using CowLibrary.Mathematics.Sampler;
using ILGPU.Algorithms.Random;

public interface ILocalSamplerProvider
{
    LocalSampler GetSampler();
}

public class LocalSamplerProvider : ILocalSamplerProvider, IDisposable
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    private RNG<XorShift64Star> rng;
    private LocalSampler sampler;

    [PostConstruct]
    public void Initialize()
    {
        var random = new Random();
        rng = RNG.Create<XorShift64Star>(GpuKernel.Accelerator, random);
        var rngView = rng.GetView(GpuKernel.Accelerator.WarpSize);
        sampler = new LocalSampler(rngView);
    }

    public LocalSampler GetSampler()
    {
        return sampler;
    }

    public void Dispose()
    {
        rng?.Dispose();
    }
}
