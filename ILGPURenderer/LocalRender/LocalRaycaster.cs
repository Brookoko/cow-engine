namespace ILGPURenderer;

using CowLibrary;
using Data;
using ILGPU;

public readonly struct LocalRaycaster
{
    public Raycast Raycast(in MeshView meshes, in Ray ray)
    {
        var hit = Const.Miss;
        Raycast(in meshes.boxes, in ray, ref hit);
        Raycast(in meshes.disks, in ray, ref hit);
        Raycast(in meshes.planes, in ray, ref hit);
        Raycast(in meshes.spheres, in ray, ref hit);
        Raycast(in meshes.triangleObjects, in ray, ref hit);
        RaycastTriangleMeshes(in meshes, in ray, ref hit);
        RaycastKdTrees(in meshes, in ray, ref hit);
        return new Raycast(hit, ray);
    }

    private void Raycast<T>(in ArrayView<T> meshes, in Ray ray, ref RayHit best) where T : unmanaged, IIntersectable
    {
        for (var i = 0; i < meshes.Length; i++)
        {
            meshes[i].Intersect(in ray, ref best);
        }
    }

    private void RaycastTriangleMeshes(in MeshView meshes, in Ray ray, ref RayHit best)
    {
        for (var i = 0; i < meshes.triangleMeshes.Length; i++)
        {
            meshes.triangleMeshes[i].Intersect(in ray, in meshes.triangles, ref best);
        }
    }

    private void RaycastKdTrees(in MeshView meshes, in Ray ray, ref RayHit best)
    {
        for (var i = 0; i < meshes.trees.Length; i++)
        {
            meshes.trees[i].Intersect(in ray, in meshes.triangles, in meshes.nodes, ref best);
        }
    }
}
