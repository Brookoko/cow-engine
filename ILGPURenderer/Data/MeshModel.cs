namespace ILGPURenderer.Data;

using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public readonly struct MeshModel
{
    public readonly long count;

    public readonly ArrayView1D<Box, Stride1D.Dense> boxes;
    public readonly ArrayView1D<Disk, Stride1D.Dense> disks;
    public readonly ArrayView1D<Plane, Stride1D.Dense> planes;
    public readonly ArrayView1D<Sphere, Stride1D.Dense> spheres;
    public readonly ArrayView1D<Triangle, Stride1D.Dense> triangleObjects;
    public readonly ArrayView1D<Triangle, Stride1D.Dense> triangles;
    public readonly ArrayView1D<TriangleMeshModel, Stride1D.Dense> triangleMeshes;
    public readonly ArrayView1D<KdTreeModel, Stride1D.Dense> trees;
    public readonly ArrayView1D<KdNodeModel, Stride1D.Dense> nodes;

    public MeshModel(
        ArrayView1D<Box, Stride1D.Dense> boxes,
        ArrayView1D<Disk, Stride1D.Dense> disks,
        ArrayView1D<Plane, Stride1D.Dense> planes,
        ArrayView1D<Sphere, Stride1D.Dense> spheres,
        ArrayView1D<Triangle, Stride1D.Dense> triangleObjects,
        ArrayView1D<Triangle, Stride1D.Dense> triangles,
        ArrayView1D<TriangleMeshModel, Stride1D.Dense> triangleMeshes,
        ArrayView1D<KdTreeModel, Stride1D.Dense> trees,
        ArrayView1D<KdNodeModel, Stride1D.Dense> nodes)
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
