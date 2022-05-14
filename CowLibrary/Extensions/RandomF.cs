namespace CowLibrary;

using System;
using System.Numerics;
using System.Threading;

public static class RandomF
{
    private static int seed = Environment.TickCount;
    private static readonly Random Random = new Random(Interlocked.Increment(ref seed));

    public static Vector2 CreateSample()
    {
        return new Vector2((float)Random.NextDouble(), (float)Random.NextDouble());
    }
}
