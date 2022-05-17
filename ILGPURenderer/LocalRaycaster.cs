namespace ILGPURenderer;

using CowLibrary;
using Data;

public readonly struct LocalRaycaster
{
    private readonly long count;

    public LocalRaycaster()
    {
        count = 3;
    }

    public LocalRaycaster(long count)
    {
        this.count = count;
    }

    public RayHit Raycast(in MeshModel meshes, in Ray ray)
    {
        var index = 0;
        var hits = new RayHit[10];
        for (var i = 0; i < 10; i++)
        {
            hits[i] = Const.Miss;
        }
        for (var i = 0; i < meshes.boxes.Length; i++)
        {
            hits[index++] = meshes.boxes[i].Intersect(in ray);
        }
        for (var i = 0; i < meshes.disks.Length; i++)
        {
            hits[index++] = meshes.disks[i].Intersect(in ray);
        }
        for (var i = 0; i < meshes.planes.Length; i++)
        {
            hits[index++] = meshes.planes[i].Intersect(in ray);
        }
        for (var i = 0; i < meshes.spheres.Length; i++)
        {
            hits[index++] = meshes.spheres[i].Intersect(in ray);
        }
        for (var i = 0; i < meshes.triangleObjects.Length; i++)
        {
            hits[index++] = meshes.triangleObjects[i].Intersect(in ray);
        }
        for (var i = 0; i < meshes.triangleMeshes.Length; i++)
        {
            hits[index++] = meshes.triangleMeshes[i].Intersect(in ray, in meshes.triangles);
        }
        for (var i = 0; i < meshes.trees.Length; i++)
        {
            hits[index++] = meshes.trees[i].Intersect(in ray, in meshes.triangles, in meshes.nodes);
        }
        var rayHit = Const.Miss;
        foreach (var hit in hits)
        {
            if (rayHit.t > hit.t)
            {
                rayHit = hit;
            }
        }
        return rayHit;
    }
}
