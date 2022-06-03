namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class SceneNode
    {
        public readonly List<RenderableObject> objects;
        public readonly List<SceneNode> children = new();
        private readonly Bound box;

        public SceneNode(List<RenderableObject> objects)
        {
            this.objects = objects;
            box = CreateBox();
        }

        private Bound CreateBox()
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
            return new Bound(min, max, -1);
        }

        public Surfel? Intersect(in Ray ray)
        {
            if (children.Count == 0 && objects.Count == 0)
            {
                return null;
            }
            if (!box.Check(in ray, out _))
            {
                return null;
            }
            return children.Count > 0 ? IntersectChildren(in ray) : IntersectObjects(in ray);
        }

        private Surfel? IntersectChildren(in Ray ray)
        {
            Surfel? surfel = null;
            foreach (var child in children)
            {
                var s = child.Intersect(in ray);
                if (!s.HasValue)
                {
                    continue;
                }
                if (!surfel.HasValue || surfel.Value.hit.t > s.Value.hit.t)
                {
                    surfel = s;
                }
            }
            return surfel;
        }

        private Surfel? IntersectObjects(in Ray ray)
        {
            var bestHit = Const.Miss;
            foreach (var obj in objects)
            {
                obj.Mesh.Intersect(in ray, ref bestHit);
            }
            if (bestHit.HasHit)
            {
                return new Surfel(bestHit, ray.direction, objects[bestHit.id].Material);
            }
            return null;
        }
    }
}
