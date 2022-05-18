namespace CowLibrary.Mathematics.Sampler;

using System.Numerics;
using ILGPU.Algorithms.Random;

public readonly struct LocalSampler : ISampler
{
    private readonly RNGView<XorShift128Plus> random;

    public LocalSampler(RNGView<XorShift128Plus> random)
    {
        this.random = random;
    }

    public Vector2 CreateSample()
    {
        return new Vector2(random.NextFloat(), random.NextFloat());
    }
}
