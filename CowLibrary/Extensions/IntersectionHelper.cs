namespace CowLibrary
{
    using System.Collections.Generic;

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
    }
}
