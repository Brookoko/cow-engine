namespace ILGPURenderer.Data;

using CowLibrary;
using CowLibrary.Object.Mesh.Views;
using CowLibrary.Views;
using ILGPU;
using ILGPU.Runtime;

public readonly struct MeshView
{
    public readonly ArrayView<Box> boxes;
    public readonly ArrayView<DiskView> disks;
    public readonly ArrayView<PlaneView> planes;
    public readonly ArrayView<SphereView> spheres;
    public readonly ArrayView<TriangleView> triangleObjects;
    public readonly ArrayView<TriangleView> triangles;
    public readonly ArrayView<TriangleMeshView> triangleMeshes;
    public readonly ArrayView<KdTreeView> trees;
    public readonly ArrayView<KdNodeView> nodes;

    public MeshView(
        ArrayView<Box> boxes,
        ArrayView<DiskView> disks,
        ArrayView<PlaneView> planes,
        ArrayView<SphereView> spheres,
        ArrayView<TriangleView> triangleObjects,
        ArrayView<TriangleView> triangles,
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
    }
}
