namespace ILGPURenderer.Data;

using CowLibrary;
using CowLibrary.Views;
using ILGPU;
using ILGPU.Runtime;

public readonly struct TriangleMeshView
{
    private readonly int trianglesOffset;
    private readonly int trianglesCount;

    public TriangleMeshView(int trianglesOffset, int trianglesCount)
    {
        this.trianglesOffset = trianglesOffset;
        this.trianglesCount = trianglesCount;
    }

    public void Intersect(in Ray ray, in ArrayView<TriangleView> triangles, ref RayHit best)
    {
        for (var i = 0; i < trianglesCount; i++)
        {
            var index = trianglesOffset + i;
            triangles[index].Intersect(in ray, ref best);
        }
    }
}
