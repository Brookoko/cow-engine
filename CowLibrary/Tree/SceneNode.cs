namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class SceneNode : IIntersectable
    {
        public readonly List<RenderableObject> objects;
        public readonly List<SceneNode> children = new List<SceneNode>();
        private readonly Box box;

        public SceneNode(List<RenderableObject> objects)
        {
            this.objects = objects;
            box = CreateBox();
        }

        private Box CreateBox()
        {
            var min = Vector3.One * float.MaxValue;
            var max = Vector3.One * float.MinValue;
            foreach (var b in objects.Select(obj => obj.Mesh.BoundingBox))
            {
                min.X = Math.Min(min.X, b.min.X);
                min.Y = Math.Min(min.Y, b.min.Y);
                min.Z = Math.Min(min.Z, b.min.Z);
                max.X = Math.Max(max.X, b.max.X);
                max.Y = Math.Max(max.Y, b.max.Y);
                max.Z = Math.Max(max.Z, b.max.Z);
            }
            return new Box(min, max);
        }

        public Surfel? Intersect(in Ray ray)
        {
            if (children.Count == 0 && objects.Count == 0)
            {
                return null;
            }
            var surfel = box.Intersect(in ray);
            if (!surfel.HasValue)
            {
                return null;
            }
            return children.Count > 0 ? IntersectChildren(in ray) : IntersectObjects(in ray);
        }

        private Surfel? IntersectChildren(in Ray ray)
        {
            return IntersectionHelper.Intersect(children, in ray);
        }

        private Surfel? IntersectObjects(in Ray ray)
        {
            Surfel? surfel = default;
            foreach (var obj in objects)
            {
                var s = obj.Mesh.Intersect(in ray);
                if (s.HasValue)
                {
                    if (!surfel.HasValue || surfel.Value.t > s.Value.t)
                    {
                        surfel = s.Value with { material = obj.Material, ray = ray.direction };
                    }
                }
            }
            return surfel;
        }
    }
}
