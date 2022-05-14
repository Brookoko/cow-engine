namespace CowLibrary
{
    using System.Linq;
    using System.Numerics;

    public readonly struct KdTree : IIntersectable
    {
        private const int MinNumberOfTriangles = 8;
        private const int MaxDepth = 16;

        public readonly KdNode root;

        public KdTree(Triangle[] triangles) : this()
        {
            root = BuildNode(in triangles, 0);
        }

        private KdNode BuildNode(in Triangle[] triangles, int depth)
        {
            if (triangles.Length <= MinNumberOfTriangles || depth >= MaxDepth)
            {
                return new KdNode(triangles);
            }
            return Split(in triangles, depth);
        }

        private KdNode Split(in Triangle[] triangles, int depth)
        {
            var splitValue = GetMedian(triangles, depth);
            var (left, middle, right) = SplitTriangle(triangles, depth, splitValue);
            if (middle.Length == triangles.Length)
            {
                return new KdNode(triangles);
            }
            var leftNode = BuildNode(left, depth + 1);
            var middleNode = BuildNode(middle, depth + 1);
            var rightNode = BuildNode(right, depth + 1);
            return new KdNode(in triangles, leftNode, middleNode, rightNode);
        }

        private float GetMedian(Triangle[] triangles, int depth)
        {
            var sortedAxis = new float[triangles.Length];
            for (var j = 0; j < triangles.Length; j++)
            {
                sortedAxis[j] = GetDimension(triangles[j].BoundingBox.center, depth);
            }
            sortedAxis = sortedAxis
                .OrderBy(v => v)
                .ToArray();
            var l = sortedAxis.Length;
            var i = (l - 1) / 2;
            return l % 2 == 0 ? (sortedAxis[i] + sortedAxis[i + 1]) * 0.5f : sortedAxis[i];
        }

        private (Triangle[] left, Triangle[] right, Triangle[] middle) SplitTriangle(Triangle[] triangles, int depth,
            float v)
        {
            var leftTriangles = new Triangle[triangles.Length];
            var middleTriangles = new Triangle[triangles.Length];
            var rightTriangles = new Triangle[triangles.Length];
            var leftCount = 0;
            var middleCount = 0;
            var rightCount = 0;
            foreach (var t in triangles)
            {
                if (GetDimension(t.BoundingBox.max, depth) <= v)
                {
                    leftTriangles[leftCount++] = t;
                }
                else if (GetDimension(t.BoundingBox.min, depth) >= v)
                {
                    rightTriangles[rightCount++] = t;
                }
                else
                {
                    middleTriangles[middleCount++] = t;
                }
            }
            return (
                leftTriangles.Take(leftCount).ToArray(),
                middleTriangles.Take(middleCount).ToArray(),
                rightTriangles.Take(rightCount).ToArray());
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
