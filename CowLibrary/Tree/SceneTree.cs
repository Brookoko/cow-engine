namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class SceneTree
    {
        private const int MinNumberOfObjects = 8;
        private const int MaxDepth = 16;

        public readonly SceneNode root;

        public SceneTree(List<RenderableObject> objects)
        {
            root = BuildNode(objects, 0);
        }

        private SceneNode BuildNode(List<RenderableObject> objects, int depth)
        {
            var node = new SceneNode(objects);
            return objects.Count <= MinNumberOfObjects || depth >= MaxDepth ? node : Split(node, depth);
        }

        private SceneNode Split(SceneNode node, int depth)
        {
            var splitValue = GetMedian(node.objects, depth);
            var (left, right, middle) = SplitObjects(node.objects, depth, splitValue);
            if (middle.Count == node.objects.Count)
            {
                return node;
            }
            var leftNode = BuildNode(left, depth + 1);
            var middleNode = BuildNode(middle, depth + 1);
            var rightNode = BuildNode(right, depth + 1);
            node.children.Add(leftNode);
            node.children.Add(middleNode);
            node.children.Add(rightNode);
            return node;
        }

        private float GetMedian(List<RenderableObject> objects, int depth)
        {
            var sortedAxis = objects
                .Select(obj => GetDimension(obj.Mesh.GetBoundingBox().center, depth))
                .OrderBy(v => v)
                .ToArray();
            var l = sortedAxis.Length;
            var i = (l - 1) / 2;
            return l % 2 == 0 ? (sortedAxis[i] + sortedAxis[i + 1]) * 0.5f : sortedAxis[i];
        }

        private (List<RenderableObject> left, List<RenderableObject> right, List<RenderableObject> middle) SplitObjects(
            List<RenderableObject> objects, int depth, float v)
        {
            var leftObjects = new List<RenderableObject>();
            var rightObjects = new List<RenderableObject>();
            var middleObjects = new List<RenderableObject>();
            foreach (var obj in objects)
            {
                if (GetDimension(obj.Mesh.GetBoundingBox().max, depth) <= v)
                {
                    leftObjects.Add(obj);
                }
                else if (GetDimension(obj.Mesh.GetBoundingBox().min, depth) >= v)
                {
                    rightObjects.Add(obj);
                }
                else
                {
                    middleObjects.Add(obj);
                }
            }
            return (leftObjects, rightObjects, middleObjects);
        }

        private float GetDimension(Vector3 v, int depth)
        {
            return v.Get(depth % 3);
        }

        public Surfel? Intersect(in Ray ray)
        {
            return root.Intersect(in ray);
        }
    }
}
