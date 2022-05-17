namespace ILGPURenderer.Data;

using CowLibrary;

public readonly struct KdNodeView
{
    public readonly Bound bound;
    public readonly TriangleMeshView triangleMeshView;
    public readonly int index;

    public KdNodeView(Bound bound, int trianglesOffset, int trianglesCount, int index)
    {
        this.bound = bound;
        triangleMeshView = new TriangleMeshView(trianglesOffset, trianglesCount);
        this.index = index;
    }
}
