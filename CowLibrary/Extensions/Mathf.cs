namespace CowLibrary
{
    using System;
    using System.Numerics;
    using System.Threading;

    public static class Mathf
    {
        private static int seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Clamp(int value, int lower, int upper)
        {
            return Math.Max(Math.Min(value, upper), lower);
        }
        
        public static float Clamp(float value, float lower, float upper)
        {
            return Math.Max(Math.Min(value, upper), lower);
        }

        public static Vector2 CreateSample()
        {
            return new Vector2((float)Random.Value.NextDouble(), (float)Random.Value.NextDouble());
        }

        public static (float a, float b) Swap(float a, float b)
        {
            return (b, a);
        }

        public static Vector3 CosineSampleHemisphere(Vector2 sample)
        {
            var d = ConcentricSampleDisk(sample);
            var z = (float)Math.Sqrt(Math.Max(0, 1 - d.X * d.X - d.Y * d.Y));
            return new Vector3(d.X, z, d.Y);
        }

        public static Vector3 CosineSampleHemisphere(Vector3 up, Vector2 sample)
        {
            var v = CosineSampleHemisphere(sample);
            var right = Vector3.Cross(up, v);
            var forward = Vector3.Cross(right, up);
            var m = Matrix4x4Extensions.FromBasis(right, up, forward, Vector3.Zero);
            return m.MultiplyVector(v);
        }

        public static Vector2 ConcentricSampleDisk(Vector2 sample)
        {
            var offset = 2 * sample - Vector2.One;

            if (offset.X == 0 && offset.Y == 0)
            {
                return Vector2.Zero;
            }

            var r = offset.X;
            var d = offset.Y;
            if (Math.Abs(offset.X) < Math.Abs(offset.Y))
            {
                (r, d) = Swap(r, d);
            }
            var theta = Const.PiOver4 * (d / r);

            return r * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
        }
    }
}
