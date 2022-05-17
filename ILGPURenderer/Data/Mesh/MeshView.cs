namespace ILGPURenderer.Data;

using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public readonly struct MeshView
{
    public readonly long count;

    public readonly ArrayView<Box> boxes;
    public readonly ArrayView<Disk> disks;
    public readonly ArrayView<Plane> planes;
    public readonly ArrayView<Sphere> spheres;
    public readonly ArrayView<Triangle> triangleObjects;
    public readonly ArrayView<Triangle> triangles;
    public readonly ArrayView<TriangleMeshView> triangleMeshes;
    public readonly ArrayView<KdTreeView> trees;
    public readonly ArrayView<KdNodeView> nodes;

    public MeshView(
        ArrayView<Box> boxes,
        ArrayView<Disk> disks,
        ArrayView<Plane> planes,
        ArrayView<Sphere> spheres,
        ArrayView<Triangle> triangleObjects,
        ArrayView<Triangle> triangles,
        ArrayView<TriangleMeshView> triangleMeshes,
        ArrayView<KdTreeView> trees,
        ArrayView<KdNodeView> nodes)
    {
        this.boxes = boxes;
        this.disks = disks;
        this.planes = planes;
        this.spheres = spheres;
        this.triangleObjects = triangleObjects;
        this.triangles = triangles;
        this.triangleMeshes = triangleMeshes;
        this.nodes = nodes;
        this.trees = trees;
        count = boxes.Length +
                disks.Length +
                planes.Length +
                spheres.Length +
                triangleObjects.Length +
                trees.Length;
    }
}
