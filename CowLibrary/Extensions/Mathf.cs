namespace CowLibrary
{
    using System;
    using System.Numerics;

    public static class Mathf
    {
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

            float r;
            float theta;
            if (Math.Abs(offset.X) > Math.Abs(offset.Y))
            {
                r = offset.X;
                theta = Const.PiOver4 * (offset.Y / offset.X);
            }
            else
            {
                r = offset.X;
                theta = Const.PiOver2 - Const.PiOver4 * (offset.X / offset.Y);
            }

            return r * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
        }
    }
}
