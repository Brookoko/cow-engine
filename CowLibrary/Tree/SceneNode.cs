namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class SceneNode
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

        public bool Intersect(Ray ray, out Surfel surfel)
        {
            if (children.Count == 0 && objects.Count == 0)
            {
                surfel = null;
                return false;
            }
            if (!box.Intersect(ray, out surfel))
            {
                surfel = null;
                return false;
            }
            return children.Count > 0 ? IntersectChildren(ray, out surfel) : IntersectObjects(ray, out surfel);
        }
        
        private bool IntersectChildren(Ray ray, out Surfel surfel)
        {
            surfel = null;
            var intersected = false;
            foreach (var node in children)
            {
                if (node.Intersect(ray, out var s))
                {
                    if (surfel == null || surfel.t > s.t)
                    {
                        surfel = s;
                        intersected = true;
                    }
                }
            }
            return intersected;
        }
        
        private bool IntersectObjects(Ray ray, out Surfel surfel)
        {
            surfel = null;
            var intersected = false;
            foreach (var obj in objects)
            {
                if (obj.Mesh.Intersect(ray, out var s))
                {
                    intersected = true;
                    if (surfel == null || surfel.t > s.t)
                    {
                        surfel = s;
                        surfel.material = obj.Material;
                        surfel.ray = ray.direction;
                    }
                }
            }
            return intersected;
        }
    }
}