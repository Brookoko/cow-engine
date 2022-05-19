namespace CowLibrary.Mathematics.Sampler;

using System.Numerics;
using ILGPU;
using ILGPU.Algorithms.Random;

public readonly struct LocalSampler : ISampler
{
    private readonly ArrayView<XorShift64Star> randoms;

    public LocalSampler(ArrayView<XorShift64Star> randoms)
    {
        this.randoms = randoms;
    }

    public Vector2 CreateSample()
    {
        ref var random = ref randoms[0];
        random.ShiftPeriod(Warp.LaneIdx);
        var sample = new Vector2(random.NextFloat(), random.NextFloat());
        return sample;
    }
}
