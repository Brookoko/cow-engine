namespace CowLibrary
{
    using System;
    using System.Numerics;

    public static class IntersectionHelper
    {
        public static Bound CreateBound(Triangle[] triangles)
        {
            var min = Vector3.One * float.MaxValue;
            var max = Vector3.One * float.MinValue;
            foreach (var t in triangles)
            {
                var box = t.GetBoundingBox(); 
                min.X = Math.Min(min.X, box.min.X);
                min.Y = Math.Min(min.Y, box.min.Y);
                min.Z = Math.Min(min.Z, box.min.Z);
                max.X = Math.Max(max.X, box.max.X);
                max.Y = Math.Max(max.Y, box.max.Y);
                max.Z = Math.Max(max.Z, box.max.Z);
            }
            return new Bound(min, max);
        }
    }
}
