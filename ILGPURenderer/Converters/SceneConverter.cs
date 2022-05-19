namespace ILGPURenderer.Converters;

using System.Collections.Generic;
using System.Linq;
using Cowject;
using CowLibrary;
using CowLibrary.Lights.Models;
using CowLibrary.Views;
using CowRenderer;
using Data;
using ILGPU;
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
        return new SceneView(CreateMeshData(scene), CreateMaterialData(scene), CreateLightData(scene));
    }

    private MeshView CreateMeshData(Scene scene)
    {
        var meshes = scene.objects.Select(obj => obj.Mesh).ToArray();
        var boxes = LoadDerived<Box, IIntersectable>(meshes);
        var disks = LoadDerived<Disk, IIntersectable>(meshes);
        var planes = LoadDerived<Plane, IIntersectable>(meshes);
        var spheres = LoadDerived<Sphere, IIntersectable>(meshes);
        var triangleObjects = LoadDerived<TriangleView, IIntersectable>(meshes);
        var (triangles, triangleMeshes, trees, nodes) = LoadTriangles(meshes);
        return new MeshView(boxes, disks, planes, spheres, triangleObjects, triangles, triangleMeshes, trees, nodes);
    }

    private (
        ArrayView<TriangleView> triangles,
        ArrayView<TriangleMeshView> triangleMeshModels,
        ArrayView<KdTreeView> trees,
        ArrayView<KdNodeView> nodes) LoadTriangles(IMesh[] meshes)
    {
        var triangleMeshes = Utils.FindDerived<TriangleMesh, IMesh>(meshes);
        var optimizedMeshes = Utils.FindDerived<OptimizedMesh, IMesh>(meshes);
        var triangles = new List<Triangle>();
        var (trees, nodes) = LoadOptimizedMeshes(optimizedMeshes, triangles);
        var triangleMeshModels = LoadTriangleMeshes(triangleMeshes, triangles);
        var trianglesView = GpuKernel.ConvertToView(triangles.Select(t => t.View).ToArray());
        return (trianglesView, triangleMeshModels, trees, nodes);
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
        var diffuse = LoadDerived<DiffuseMaterial, IMaterial>(materials);
        var fresnel = LoadDerived<FresnelMaterial, IMaterial>(materials);
        var reflection = LoadDerived<ReflectionMaterial, IMaterial>(materials);
        var transmission = LoadDerived<TransmissionMaterial, IMaterial>(materials);
        return new MaterialView(diffuse, fresnel, reflection, transmission);
    }

    private LightView CreateLightData(Scene scene)
    {
        var lights = scene.lights.Select(l => l.Model).ToArray();
        var directional = LoadDerived<DirectionalLightModel, ILightModel>(lights);
        var point = LoadDerived<PointLightModel, ILightModel>(lights);
        var environment = LoadDerived<EnvironmentLightModel, ILightModel>(lights);
        var matrices = scene.lights.Select(l => l.Transform.LocalToWorldMatrix).ToArray();
        var matricesView = GpuKernel.ConvertToView(matrices);
        return new LightView(directional, point, environment, matricesView);
    }

    private ArrayView<TDerived> LoadDerived<TDerived, TBase>(IEnumerable<TBase> array) where TDerived : unmanaged, TBase
    {
        var derived = Utils.FindDerived<TDerived, TBase>(array);
        return GpuKernel.ConvertToView(derived);
    }
}
