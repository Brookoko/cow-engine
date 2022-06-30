namespace CowLibrary.Mathematics.Sampler;

using System;
using System.Numerics;
using System.Threading;

public class ThreadSafeSampler : ISampler
{
    private static int seed = Environment.TickCount;
    private static readonly ThreadLocal<Random> Random = new(() => new Random(Interlocked.Increment(ref seed)));

    public Vector2 CreateSample()
    {
        return new Vector2(Random.Value.NextSingle(), Random.Value.NextSingle());
    }
}
