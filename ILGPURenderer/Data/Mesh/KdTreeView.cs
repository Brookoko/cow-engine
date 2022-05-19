namespace ILGPURenderer.Data;

using CowLibrary;
using CowLibrary.Views;
using ILGPU;
using ILGPU.Runtime;

public readonly struct KdTreeView
{
    private readonly int index;

    public KdTreeView(int index)
    {
        this.index = index;
    }

    public RayHit Intersect(in Ray ray,
        in ArrayView<TriangleView> triangles,
        in ArrayView<KdNodeView> nodes)
    {
        return IsInBound(in ray, in nodes[index]) ? IntersectNodes(in ray, in triangles, in nodes) : Const.Miss;
    }

    private RayHit IntersectNodes(in Ray ray,
        in ArrayView<TriangleView> triangles,
        in ArrayView<KdNodeView> nodes)
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

            if (!IsInBound(in ray, in node))
            {
                depth--;
                nodeIndex = parent[depth];
                childNumbers[depth]++;
                continue;
            }

            if (node.leftIndex < 0)
            {
                var tHit = node.triangleMeshView.Intersect(in ray, in triangles);
                if (tHit.t < hit.t)
                {
                    hit = tHit;
                }
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

    private bool IsInBound(in Ray ray, in KdNodeView node)
    {
        var boundHit = node.bound.Intersect(in ray);
        return boundHit.HasHit;
    }

    private int GetChild(in ArrayView<KdNodeView> nodes, in int nodeIndex, in byte childNumber)
    {
        ref var node = ref nodes[nodeIndex];
        return childNumber switch
        {
            0 => node.leftIndex,
            1 => node.middleIndex,
            _ => node.rightIndex
        };
    }
}
