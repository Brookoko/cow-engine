namespace ILGPURenderer.Data;

using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public readonly struct TriangleMeshModel
{
    private readonly int trianglesOffset;
    private readonly int trianglesCount;

    public TriangleMeshModel(int trianglesOffset, int trianglesCount)
    {
        this.trianglesOffset = trianglesOffset;
        this.trianglesCount = trianglesCount;
    }

    public readonly RayHit Intersect(in Ray ray, in ArrayView1D<Triangle, Stride1D.Dense> triangles)
    {
        var hit = new RayHit();
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
