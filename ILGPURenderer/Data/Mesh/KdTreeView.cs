namespace ILGPURenderer.Data;

using CowLibrary;
using CowLibrary.Views;
using ILGPU;

public readonly struct KdTreeView
{
    private readonly int index;

    public KdTreeView(int index)
    {
        this.index = index;
    }

    public void Intersect(in Ray ray,
        in ArrayView<TriangleView> triangles,
        in ArrayView<KdNodeView> nodes,
        ref RayHit best)
    {
        if (IsInBound(in ray, in nodes[index], ref best))
        {
            IntersectNodes(in ray, in triangles, in nodes, ref best);
        }
    }

    private RayHit IntersectNodes(in Ray ray,
        in ArrayView<TriangleView> triangles,
        in ArrayView<KdNodeView> nodes,
        ref RayHit best)
    {
        var childNumbers = new byte[Const.MaxDepth + 1];
        var parent = new int[Const.MaxDepth + 1];
        var depth = 0;
        var nodeIndex = 0;
        var hit = Const.Miss;

        while (childNumbers[0] < Const.KdNodeCount)
        {
            var offsetIndex = nodeIndex + index;
            var node = nodes[offsetIndex];

            if (!IsInBound(in ray, in node, ref best))
            {
                if (depth == 0)
                {
                    break;
                }
                depth--;
                nodeIndex = parent[depth];
                childNumbers[depth]++;
                continue;
            }

            if (node.leftIndex < 0)
            {
                node.triangleMeshView.Intersect(in ray, in triangles, ref best);
                depth--;
                nodeIndex = parent[depth];
                childNumbers[depth]++;
                continue;
            }

            if (childNumbers[depth] < Const.KdNodeCount)
            {
                parent[depth] = nodeIndex;
                nodeIndex = GetChild(in nodes, in nodeIndex, in childNumbers[depth]);
                depth++;
                childNumbers[depth] = 0;
            }
            else
            {
                depth--;
                nodeIndex = parent[depth];
                childNumbers[depth]++;
            }
        }
        return hit;
    }

    private bool IsInBound(in Ray ray, in KdNodeView node, ref RayHit best)
    {
        return node.bound.Check(in ray, out var t) && t < best.t;
    }

    private int GetChild(in ArrayView<KdNodeView> nodes, in int nodeIndex, in byte childNumber)
    {
        ref var node = ref nodes[nodeIndex];
        return childNumber switch
        {
            0 => node.rightIndex,
            1 => node.middleIndex,
            _ => node.leftIndex
        };
    }
}
