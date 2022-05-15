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
        // foreach (var mesh in meshes.optimizedMeshes)
        // {
        //     hits[index++] = mesh.Intersect(in ray, out var hit) ? hit : new RayHit();
        // }
        var rayHit = new RayHit();
        // for (var i = 0; i < hits.Length; i++)
        // {
        //     var hit = hits[i];
        //     if (rayHit.t > hit.t)
        //     {
        //         rayHit = hit;
        //     }
        // }
        return rayHit;
    }
}
