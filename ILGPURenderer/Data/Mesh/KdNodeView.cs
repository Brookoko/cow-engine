namespace ILGPURenderer.Data;

using CowLibrary;

public readonly struct KdNodeView
{
    public readonly Bound bound;
    public readonly TriangleMeshView triangleMeshView;
    public readonly int leftIndex;
    public readonly int middleIndex;
    public readonly int rightIndex;

    public KdNodeView(Bound bound, int trianglesOffset, int trianglesCount,
        int leftIndex, int middleIndex, int rightIndex)
    {
        this.bound = bound;
        triangleMeshView = new TriangleMeshView(trianglesOffset, trianglesCount);
        this.leftIndex = leftIndex;
        this.middleIndex = middleIndex;
        this.rightIndex = rightIndex;
    }
}
