namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class KdTree
    {
        private const int MinNumberOfTriangles = 8;
        
        public readonly KdNode root;
        
        public KdTree(List<Triangle> triangles)
        {
            root = BuildNode(triangles, 0);
        }
        
        private KdNode BuildNode(List<Triangle> triangles, int depth)
        {
            var node = new KdNode(triangles);
            return triangles.Count <= MinNumberOfTriangles ? node : Split(node, depth);
        }
        
        private KdNode Split(KdNode node, int depth)
        {
            var splitValue = GetMedian(node.mesh.triangles, depth);
            var (left, right, middle) = SplitTriangle(node.mesh.triangles, depth, splitValue);
            if (middle.Count == node.mesh.triangles.Count)
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

        private float GetMedian(List<Triangle> triangles, int depth)
        {
            var sortedAxis = triangles
                .Select(t => GetDimension(t.BoundingBox.center, depth))
                .OrderBy(v => v)
                .ToArray();
            var l = sortedAxis.Length;
            var i = (l - 1) / 2;
            return l % 2 == 0 ? (sortedAxis[i] + sortedAxis[i + 1]) * 0.5f : sortedAxis[i];
        }
        
        private (List<Triangle> left, List<Triangle> right, List<Triangle> middle) SplitTriangle(List<Triangle> triangles, int depth, float v)
        {
            var leftTriangles = triangles
                .Where(t => GetDimension(t.BoundingBox.max, depth) <= v)
                .ToList();
            var rightTriangles = triangles
                .Where(t => GetDimension(t.BoundingBox.min, depth) >= v)
                .ToList();
            var middleTriangles = triangles
                .Where(t => !leftTriangles.Contains(t) && !rightTriangles.Contains(t))
                .ToList();
            return (leftTriangles, rightTriangles, middleTriangles);
        }
        
        private float GetDimension(Vector3 v, int depth)
        {
            depth %= 3;
            return depth == 0 ? v.X : depth == 1 ? v.Y : v.Z;
        }
        
        public bool Intersect(Ray ray, out Surfel surfel)
        {
            return root.Intersect(ray, out surfel);
        }
    }
}