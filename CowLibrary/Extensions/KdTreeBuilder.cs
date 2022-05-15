namespace CowLibrary;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class KdTreeBuilder
{
    private const int MinNumberOfTriangles = 8;
    private const int MaxDepth = 8;

    public static KdTree Build(in Triangle[] triangles)
    {
        var nodes = new List<KdNode>();
        BuildNode(in triangles, nodes, 0, 0);
        return new KdTree(nodes.ToArray());
    }

    private static void BuildNode(in Triangle[] triangles, List<KdNode> nodes, int depth, int count)
    {
        if (triangles.Length <= MinNumberOfTriangles || depth >= MaxDepth)
        {
            AddNode(nodes, new KdNode(triangles), count);
            return;
        }
        Split(in triangles, nodes, depth, count);
    }

    private static void Split(in Triangle[] triangles, List<KdNode> nodes, int depth, int count)
    {
        var splitValue = GetMedian(triangles, depth);
        var (left, middle, right) = SplitTriangle(triangles, depth, splitValue);
        if (middle.Length == triangles.Length)
        {
            AddNode(nodes, new KdNode(triangles), count);
            return;
        }
        var node = new KdNode(in triangles, count);
        AddNode(nodes, node, count);
        BuildNode(left, nodes, depth + 1, 3 * count + 1);
        BuildNode(middle, nodes, depth + 1, 3 * count + 2);
        BuildNode(right, nodes, depth + 1, 3 * count + 3);
    }

    private static void AddNode(List<KdNode> nodes, in KdNode node, int count)
    {
        var nodesToFill = count + 1 - nodes.Count;
        if (nodesToFill > 0)
        {
            for (var i = 0; i < nodesToFill; i++)
            {
                nodes.Add(new KdNode());
            }
        }
        nodes[count] = node;
    }

    private static float GetMedian(Triangle[] triangles, int depth)
    {
        var sortedAxis = new float[triangles.Length];
        for (var j = 0; j < triangles.Length; j++)
        {
            sortedAxis[j] = GetDimension(triangles[j].GetBoundingBox().center, depth);
        }
        sortedAxis = sortedAxis
            .OrderBy(v => v)
            .ToArray();
        var l = sortedAxis.Length;
        var i = (l - 1) / 2;
        return l % 2 == 0 ? (sortedAxis[i] + sortedAxis[i + 1]) * 0.5f : sortedAxis[i];
    }

    private static (Triangle[] left, Triangle[] right, Triangle[] middle) SplitTriangle(Triangle[] triangles, int depth,
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
            if (GetDimension(t.GetBoundingBox().max, depth) <= v)
            {
                leftTriangles[leftCount++] = t;
            }
            else if (GetDimension(t.GetBoundingBox().min, depth) >= v)
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

    private static float GetDimension(Vector3 v, int depth)
    {
        return v.Get(depth % 3);
    }
}
