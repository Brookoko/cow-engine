namespace CowLibrary
{
    using System;
    using System.Numerics;

    public static class Vector3Extensions
    {
        public static Vector3 Normalize(this Vector3 v)
        {
            return Vector3.Normalize(v);
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
    }
}