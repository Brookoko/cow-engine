namespace CowLibrary;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class KdTreeBuilder
{
    public static KdTree Build(in Triangle[] triangles, int id)
    {
        var nodes = new List<KdNode>();
        var count = 0;
        BuildNode(in triangles, nodes, 0, ref count, id);
        var reverted = new KdNode[nodes.Count];
        for (var i = nodes.Count - 1; i >= 0; i--)
        {
            var node = nodes[i];
            var index = Revert(i);
            var leftIndex = Revert(node.leftIndex);
            var middleIndex = Revert(node.middleIndex);
            var rightIndex = Revert(node.rightIndex);
            reverted[index] = node.Copy(leftIndex, middleIndex, rightIndex);
        }
        return new KdTree(reverted);

        int Revert(int index)
        {
            return index < 0 ? index : nodes.Count - index - 1;
        }
    }

    private static void BuildNode(in Triangle[] triangles, List<KdNode> nodes, int depth, ref int count, int id)
    {
        if (triangles.Length <= Const.MinNumberOfTriangles || depth >= Const.MaxDepth)
        {
            nodes.Add(new KdNode(triangles, id));
            count++;
            return;
        }
        Split(in triangles, nodes, depth, ref count, id);
    }

    private static void Split(in Triangle[] triangles, List<KdNode> nodes, int depth, ref int count, int id)
    {
        var splitValue = GetMedian(triangles, depth);
        var (left, middle, right) = SplitTriangle(triangles, depth, splitValue);
        if (middle.Length == triangles.Length)
        {
            nodes.Add(new KdNode(triangles, id));
            count++;
            return;
        }
        BuildNode(left, nodes, depth + 1, ref count, id);
        var leftIndex = count - 1;
        BuildNode(middle, nodes, depth + 1, ref count, id);
        var middleIndex = count - 1;
        BuildNode(right, nodes, depth + 1, ref count, id);
        var rightIndex = count - 1;
        var node = new KdNode(in triangles, leftIndex, middleIndex, rightIndex);
        nodes.Add(node);
        count++;
    }

    private static float GetMedian(Triangle[] triangles, int depth)
    {
        var sortedAxis = new float[triangles.Length];
        for (var j = 0; j < triangles.Length; j++)
        {
            sortedAxis[j] = GetDimension(triangles[j].BoundingBox.Center, depth);
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

    private static float GetDimension(Vector3 v, int depth)
    {
        return v.Get(depth % 3);
    }
}
