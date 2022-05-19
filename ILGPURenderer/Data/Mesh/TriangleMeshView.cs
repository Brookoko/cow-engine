namespace ILGPURenderer.Data;

using CowLibrary;
using CowLibrary.Views;
using ILGPU;
using ILGPU.Runtime;

public readonly struct TriangleMeshView
{
    public readonly int trianglesOffset;
    public readonly int trianglesCount;

    public TriangleMeshView(int trianglesOffset, int trianglesCount)
    {
        this.trianglesOffset = trianglesOffset;
        this.trianglesCount = trianglesCount;
    }

    public RayHit Intersect(in Ray ray, in ArrayView<TriangleView> triangles)
    {
        var hit = Const.Miss;
        for (var i = 0; i < trianglesCount; i++)
        {
            var index = trianglesOffset + i;
            var tHit = triangles[index].Intersect(in ray);
            if (hit.t > tHit.t)
            {
                hit = tHit;
            }
        }
        return hit;
    }
}
