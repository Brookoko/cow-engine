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
            root = BuildNode(triangles, 0);
        }

        private KdNode BuildNode(Triangle[] triangles, int depth)
        {
            var node = new KdNode(triangles);
            return triangles.Length <= MinNumberOfTriangles || depth >= MaxDepth ? node : Split(node, depth);
        }

        private KdNode Split(KdNode node, int depth)
        {
            var splitValue = GetMedian(node.mesh.triangles, depth);
            var (left, middle, right) = SplitTriangle(node.mesh.triangles, depth, splitValue);
            if (middle.Length == node.mesh.triangles.Length)
            {
                return node;
            }
            var leftNode = BuildNode(left, depth + 1);
            var middleNode = BuildNode(middle, depth + 1);
            var rightNode = BuildNode(right, depth + 1);
            node.children[0] = leftNode;
            node.children[1] = middleNode;
            node.children[2] = rightNode;
            return node;
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
            return (leftTriangles, middleTriangles, rightTriangles);
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
