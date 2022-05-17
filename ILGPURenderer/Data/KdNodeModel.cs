namespace ILGPURenderer.Data;

using CowLibrary;

public readonly struct KdNodeModel
{
    public readonly Bound bound;
    public readonly int index;
    public readonly TriangleMeshModel triangleMeshModel;

    public KdNodeModel(Bound bound, int index, int trianglesOffset, int trianglesCount)
    {
        this.bound = bound;
        this.index = index;
        triangleMeshModel = new TriangleMeshModel(trianglesOffset, trianglesCount);
    }
}
