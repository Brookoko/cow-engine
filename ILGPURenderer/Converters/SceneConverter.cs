namespace ILGPURenderer.Converters;

using System.Collections.Generic;
using System.Linq;
using Cowject;
using CowLibrary;
using CowRenderer;
using Data;
using ILGPU;
using ILGPU.Runtime;
using Utils;

public interface ISceneConverter
{
    SceneView Convert(Scene scene);
}

public class SceneConverter : ISceneConverter
{
    [Inject]
    public GpuKernel GpuKernel { get; set; }

    public SceneView Convert(Scene scene)
    {
        return new SceneView(CreateMeshData(scene), CreateMaterialData(scene));
    }

    private MeshView CreateMeshData(Scene scene)
    {
        var meshes = scene.objects.Select(obj => obj.Mesh).ToArray();
        var boxes = LoadMesh<Box>(meshes);
        var disks = LoadMesh<Disk>(meshes);
        var planes = LoadMesh<Plane>(meshes);
        var spheres = LoadMesh<Sphere>(meshes);
        var triangleObjects = LoadMesh<Triangle>(meshes);
        var (triangles, triangleMeshes, trees, nodes) = LoadTriangles(meshes);
        return new MeshView(boxes, disks, planes, spheres, triangleObjects, triangles, triangleMeshes, trees, nodes);
    }

    private (
        ArrayView<Triangle> triangles,
        ArrayView<TriangleMeshView> triangleMeshModels,
        ArrayView<KdTreeView> trees,
        ArrayView<KdNodeView> nodes) LoadTriangles(IMesh[] meshes)
    {
        var triangleMeshes = Utils.FindDerived<TriangleMesh, IMesh>(meshes);
        var optimizedMeshes = Utils.FindDerived<OptimizedMesh, IMesh>(meshes);
        var triangles = new List<Triangle>();
        var (trees, nodes) = LoadOptimizedMeshes(optimizedMeshes, triangles);
        var triangleMeshModels = LoadTriangleMeshes(triangleMeshes, triangles);
        return (GpuKernel.ConvertToView(triangles.ToArray()), triangleMeshModels, trees, nodes);
    }

    private (ArrayView<KdTreeView> trees, ArrayView<KdNodeView> nodes) LoadOptimizedMeshes(
        OptimizedMesh[] optimizedMeshes, List<Triangle> triangles)
    {
        var trees = new List<KdTreeView>();
        var nodes = new List<KdNodeView>();
        var triangleOffset = triangles.Count;
        var indexOffset = 0;
        for (var i = 0; i < optimizedMeshes.Length; i++)
        {
            var tree = optimizedMeshes[i].tree;
            foreach (var node in tree.nodes)
            {
                if (node.mesh.triangles != null)
                {
                    triangles.AddRange(node.mesh.triangles);
                }
                var trianglesLength = node.mesh.triangles?.Length ?? 0;
                var model = new KdNodeView(node.bound, triangleOffset, trianglesLength, node.index);
                nodes.Add(model);
                triangleOffset += trianglesLength;
            }
            var treeModel = new KdTreeView(indexOffset);
            trees.Add(treeModel);
            indexOffset += tree.nodes.Length;
        }
        return (GpuKernel.ConvertToView(trees.ToArray()), GpuKernel.ConvertToView(nodes.ToArray()));
    }

    private ArrayView<TriangleMeshView> LoadTriangleMeshes(TriangleMesh[] triangleMeshes, List<Triangle> triangles)
    {
        var triangleMeshModels = new TriangleMeshView[triangleMeshes.Length];
        var offset = triangles.Count;
        for (var i = 0; i < triangleMeshes.Length; i++)
        {
            var triangleMesh = triangleMeshes[i];
            var count = triangleMesh.triangles.Length;
            var view = new TriangleMeshView(offset, count);
            triangles.AddRange(triangleMesh.triangles);
            offset += count;
            triangleMeshModels[i] = view;
        }
        return GpuKernel.ConvertToView(triangleMeshModels);
    }

    private MaterialView CreateMaterialData(Scene scene)
    {
        var materials = scene.objects.Select(obj => obj.Material).ToArray();
        var diffuse = materials.Where(m => m is DiffuseMaterial).Cast<DiffuseMaterial>().ToArray();
        var fresnel = materials.Where(m => m is FresnelMaterial).Cast<FresnelMaterial>().ToArray();
        var reflection = materials.Where(m => m is ReflectionMaterial).Cast<ReflectionMaterial>().ToArray();
        var transmission = materials.Where(m => m is TransmissionMaterial).Cast<TransmissionMaterial>().ToArray();
        return new MaterialView(diffuse, fresnel, reflection, transmission);
    }

    private ArrayView1D<T, Stride1D.Dense> LoadMesh<T>(IMesh[] meshes) where T : unmanaged, IMesh
    {
        var foundMeshes = Utils.FindDerived<T, IMesh>(meshes);
        return GpuKernel.ConvertToView(foundMeshes);
    }
}
