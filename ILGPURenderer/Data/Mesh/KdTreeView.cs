namespace ILGPURenderer.Data;

using CowLibrary;
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
        in ArrayView1D<Triangle, Stride1D.Dense> triangles,
        in ArrayView1D<KdNodeView, Stride1D.Dense> nodes)
    {
        return IsInBound(in ray, in nodes[index]) ? IntersectNodes(in ray, in triangles, in nodes) : Const.Miss;
    }

    private RayHit IntersectNodes(in Ray ray,
        in ArrayView1D<Triangle, Stride1D.Dense> triangles,
        in ArrayView1D<KdNodeView, Stride1D.Dense> nodes)
    {
        var childNumbers = new short[Const.MaxDepth + 1];
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
                nodeIndex = (nodeIndex - childNumbers[depth] + 1) / Const.KdNodeCount;
                childNumbers[depth]++;
                continue;
            }

            if (node.index < 0)
            {
                var tHit = node.triangleMeshView.Intersect(in ray, in triangles);
                if (tHit.t < hit.t)
                {
                    hit = tHit;
                }
                depth--;
                nodeIndex = (nodeIndex - childNumbers[depth] + 1) / Const.KdNodeCount;
                childNumbers[depth]++;
                continue;
            }

            if (childNumbers[depth] < Const.KdNodeCount)
            {
                nodeIndex = Const.KdNodeCount * nodeIndex + childNumbers[depth] + 1;
                depth++;
                childNumbers[depth] = 0;
            }
            else
            {
                depth--;
                nodeIndex = (nodeIndex - childNumbers[depth] + 1) / Const.KdNodeCount;
                childNumbers[depth]++;
            }
        }
        return hit;
    }

    private bool IsInBound(in Ray ray, in KdNodeView node)
    {
        if (node.index < 0 && node.triangleMeshView.trianglesCount == 0)
        {
            return false;
        }
        var boundHit = node.bound.Intersect(in ray);
        return boundHit.HasHit;
    }
}
