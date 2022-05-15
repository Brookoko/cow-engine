namespace CowLibrary.Mathematics.Sampler;

using System.Numerics;
using ILGPU.Algorithms.Random;

public readonly struct LocalSampler : ISampler
{
    private readonly RNGView<XorShift64Star> random;

    public LocalSampler(RNGView<XorShift64Star> random)
    {
        this.random = random;
    }

    public Vector2 CreateSample()
    {
        return new Vector2(random.NextFloat(), random.NextFloat());
    }
}
