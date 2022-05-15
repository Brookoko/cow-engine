namespace ILGPURenderer.Data;

using CowLibrary;
using ILGPU;
using ILGPU.Runtime;

public readonly struct MeshModel
{
    public long Count => boxes.Length +
                         disks.Length +
                         planes.Length +
                         spheres.Length +
                         triangleObjects.Length;

    public readonly ArrayView1D<Box, Stride1D.Dense> boxes;
    public readonly ArrayView1D<Disk, Stride1D.Dense> disks;
    public readonly ArrayView1D<Plane, Stride1D.Dense> planes;
    public readonly ArrayView1D<Sphere, Stride1D.Dense> spheres;
    public readonly ArrayView1D<Triangle, Stride1D.Dense> triangleObjects;
    public readonly ArrayView1D<Triangle, Stride1D.Dense> triangles;
    public readonly ArrayView1D<TriangleMeshModel, Stride1D.Dense> triangleMeshes;

    // public readonly ArrayView1D<OptimizedMesh, Stride1D.Dense> optimizedMeshes;

    public MeshModel(
        ArrayView1D<Box, Stride1D.Dense> boxes,
        ArrayView1D<Disk, Stride1D.Dense> disks,
        ArrayView1D<Plane, Stride1D.Dense> planes,
        ArrayView1D<Sphere, Stride1D.Dense> spheres,
        ArrayView1D<Triangle, Stride1D.Dense> triangleObjects,
        ArrayView1D<Triangle, Stride1D.Dense> triangles,
        ArrayView1D<TriangleMeshModel, Stride1D.Dense> triangleMeshes
        // ArrayView1D<TriangleMesh, Stride1D.Dense> triangleMeshes,
        // ArrayView1D<OptimizedMesh, Stride1D.Dense> optimizedMeshes,
    )
    {
        this.boxes = boxes;
        this.disks = disks;
        this.planes = planes;
        this.spheres = spheres;
        this.triangleObjects = triangleObjects;
        this.triangles = triangles;
        this.triangleMeshes = triangleMeshes;
        // this.triangleMeshes = triangleMeshes;
        // this.optimizedMeshes = optimizedMeshes;
    }
}
