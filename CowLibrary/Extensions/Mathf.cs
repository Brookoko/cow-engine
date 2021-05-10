namespace CowLibrary
{
    using System;
    using System.Numerics;
    using System.Threading;

    public static class Mathf
    {
        private static int seed = Environment.TickCount;
        
        private static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        
        public static Vector2 CreateSample()
        {
            return new Vector2((float) random.Value.NextDouble(), (float) random.Value.NextDouble());
        }
        
        public static Vector3 OnUnitSphere()
        {
            var cosTheta = (float) (random.Value.NextDouble() * 2 - 1);
            var sinTheta = (float) Math.Sqrt(1 - cosTheta * cosTheta);
            var sinPhi = (float) (random.Value.NextDouble() * 2 - 1);
            var cosPhi = (float) Math.Sqrt(1 - sinPhi * sinPhi);
            return new Vector3(sinPhi * cosTheta, sinPhi * sinTheta, cosPhi);
        }
        
        public static Vector3 OnUnitHalfSphere(Vector3 up)
        {
            return OnUnitHalfSphere(up, CreateSample());
        }
        
        public static Vector3 OnUnitHalfSphere(Vector3 up, Vector2 sample)
        {
            var phi = sample.Y * 360;
            var cosPhi = (float) Math.Cos(phi);
            var sinPhi = (float) Math.Sin(phi);
            var cosTheta = sample.X;
            var sinTheta = (float) Math.Sqrt(1 - cosTheta * cosTheta);

            var m = Matrix4x4.Identity;
            m.M11 = cosPhi * cosTheta;
            m.M12 = -cosPhi * sinTheta;
            m.M13 = sinPhi;
            m.M21 = sinTheta;
            m.M22 = cosTheta;
            m.M23 = 0;
            m.M31 = -sinPhi * cosTheta;
            m.M32 = sinPhi * sinTheta;
            m.M33 = cosPhi;
            
            return m.MultiplyVector(up);
        }
        
        public static (float a, float b) Swap(float a, float b)
        {
            return (b, a);
        }
        
        public static Vector3 CosineSampleHemisphere(Vector2 sample)
        {
            var d = ConcentricSampleDisk(sample);
            var z = (float) Math.Sqrt(Math.Max(0, 1 - d.X * d.X - d.Y * d.Y));
            return new Vector3(d.X, d.Y, z);
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

            return r * new Vector2((float) Math.Cos(theta), (float) Math.Sin(theta));
        }
    }
}