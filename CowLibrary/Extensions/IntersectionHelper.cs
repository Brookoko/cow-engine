namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public static class IntersectionHelper
    {
        public static Surfel? Intersect(IEnumerable<IIntersectable> intersectables, in Ray ray)
        {
            Surfel? surfel = null;
            foreach (var intersectable in intersectables)
            {
                var s = intersectable.Intersect(in ray);
                if (s.HasValue)
                {
                    if (!surfel.HasValue || surfel.Value.t > s.Value.t)
                    {
                        surfel = s;
                    }
                }
            }
            return surfel;
        }

        public static Bound CreateBound(Triangle[] triangles)
        {
            var min = Vector3.One * float.MaxValue;
            var max = Vector3.One * float.MinValue;
            foreach (var t in triangles)
            {
                var box = t.BoundingBox; 
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
