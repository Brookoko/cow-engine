namespace CowLibrary
{
    using System;
    using System.Numerics;

    public static class Vector3Extensions
    {
        public static float Min(this Vector3 v)
        {
            return Math.Min(v.X, Math.Min(v.Y, v.Z));
        }

        public static Vector3 Normalize(this Vector3 v)
        {
            return Vector3.Normalize(v);
        }

        public static Vector2 Normalize(this Vector2 v)
        {
            return Vector2.Normalize(v);
        }

        public static double AngleTo(this Vector3 a, Vector3 b)
        {
            return a.AngleRadTo(b) * Const.Rad2Deg;
        }

        public static double AngleRadTo(this Vector3 a, Vector3 b)
        {
            return Math.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
        }

        public static float Get(this Vector3 v, int i)
        {
            return i == 0 ? v.X : i == 1 ? v.Y : v.Z;
        }

        public static Vector3 Reflect(this Vector3 v, Vector3 n)
        {
            var num = -2f * Vector3.Dot(n, v);
            return new Vector3(num * n.X + v.X, num * n.Y + v.Y, num * n.Z + v.Z);
        }

        public static Vector3 Faceforward(Vector3 n, Vector3 v)
        {
            return Vector3.Dot(n, v) < 0 ? n : -n;
        }

        public static bool Refract(this Vector3 v, Vector3 n, float eta, out Vector3 w)
        {
            var cosThetaI = Vector3.Dot(n, v);
            var sin2ThetaI = Math.Max(0, 1 - cosThetaI * cosThetaI);
            var sin2ThetaT = eta * eta * sin2ThetaI;
            if (sin2ThetaT >= 1)
            {
                w = Vector3.Zero;
                return false;
            }
            var cosThetaT = (float)Math.Sqrt(1 - sin2ThetaT);

            w = eta * -v + (eta * cosThetaI - cosThetaT) * n;
            return true;
        }
    }
}
