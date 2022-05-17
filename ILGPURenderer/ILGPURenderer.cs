namespace ILGPURenderer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cowject;
    using CowLibrary;
    using CowRenderer;
    using Data;
    using ILGPU;
    using ILGPU.Runtime;

    public class ILGPURenderer : IRenderer
    {
        [Inject]
        public GpuKernel GpuKernel { get; set; }

        [Inject]
        public IPrimaryRayGenerator PrimaryRayGenerator { get; set; }

        [Inject]
        public IHitGenerator HitGenerator { get; set; }

        [Inject]
        public IColorGenerator ColorGenerator { get; set; }

        public Image Render(Scene scene)
        {
            var camera = scene.MainCamera;
            var w = camera.Width;
            var h = camera.Height;
            var image = new Image(w, h);
            var sceneData = CreateSceneData(scene);
            var rays = PrimaryRayGenerator.GeneratePrimaryRays(camera);
            GpuKernel.Accelerator.Synchronize();
            var hits = HitGenerator.GenerateHits(sceneData, rays);
            GpuKernel.Accelerator.Synchronize();
            var colorsBuffer = ColorGenerator.GenerateColors(hits);
            GpuKernel.Accelerator.Synchronize();
            var colors = colorsBuffer.GetAsArray3D();
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    image[j, i] = colors[i, j, 0];
                }
            }
            return image;
        }

        private SceneModel CreateSceneData(Scene scene)
        {
            return new SceneModel(CreateMeshData(scene), CreateMaterialData(scene));
        }

        private MeshModel CreateMeshData(Scene scene)
        {
            var meshes = scene.objects.Select(obj => obj.Mesh).ToArray();
            var boxes = LoadMesh<Box>(meshes);
            var disks = LoadMesh<Disk>(meshes);
            var planes = LoadMesh<Plane>(meshes);
            var spheres = LoadMesh<Sphere>(meshes);
            var triangleObjects = LoadMesh<Triangle>(meshes);
            var (triangles, triangleMeshes, trees, nodes) = LoadTriangles(meshes);
            var trianglesView = ConvertToView(triangles.ToArray());
            var triangleMeshesView = ConvertToView(triangleMeshes.ToArray());
            var treesView = ConvertToView(trees.ToArray());
            var nodesView = ConvertToView(nodes.ToArray());
            return new MeshModel(boxes, disks, planes, spheres, triangleObjects, trianglesView, triangleMeshesView,
                treesView, nodesView);
        }

        private ArrayView1D<T, Stride1D.Dense> LoadMesh<T>(IMesh[] meshes) where T : unmanaged, IMesh
        {
            var foundMeshes = FindMeshes<T>(meshes);
            return ConvertToView(foundMeshes);
        }

        private (
            List<Triangle> triangles,
            List<TriangleMeshModel> triangleMeshModels,
            List<KdTreeModel> trees,
            List<KdNodeModel> nodes) LoadTriangles(IMesh[] meshes)
        {
            var triangleMeshes = FindMeshes<TriangleMesh>(meshes);
            var optimizedMeshes = FindMeshes<OptimizedMesh>(meshes);
            var triangles = new List<Triangle>();
            var (trees, nodes) = LoadOptimizedMeshes(optimizedMeshes, triangles);
            var triangleMeshModels = LoadTriangleMeshes(triangleMeshes, triangles);
            return (triangles, triangleMeshModels, trees, nodes);
        }

        private (List<KdTreeModel> trees, List<KdNodeModel> nodes) LoadOptimizedMeshes(
            OptimizedMesh[] optimizedMeshes, List<Triangle> triangles)
        {
            var trees = new List<KdTreeModel>();
            var nodes = new List<KdNodeModel>();
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
                    var model = new KdNodeModel(node.bound, node.index, triangleOffset, trianglesLength);
                    nodes.Add(model);
                    triangleOffset += trianglesLength;
                }
                var treeModel = new KdTreeModel(indexOffset);
                trees.Add(treeModel);
                indexOffset += tree.nodes.Length;
            }
            return (trees, nodes);
        }

        private List<TriangleMeshModel> LoadTriangleMeshes(TriangleMesh[] triangleMeshes, List<Triangle> triangles)
        {
            var triangleMeshModels = new List<TriangleMeshModel>(triangleMeshes.Length);
            var offset = triangles.Count;
            foreach (var triangleMesh in triangleMeshes)
            {
                var count = triangleMesh.triangles.Length;
                var model = new TriangleMeshModel(offset, count);
                triangleMeshModels.Add(model);
                triangles.AddRange(triangleMesh.triangles);
                offset += count;
            }
            return triangleMeshModels;
        }

        private T[] FindMeshes<T>(IMesh[] meshes) where T : struct, IMesh
        {
            return meshes.Where(m => m is T).Cast<T>().ToArray();
        }

        private ArrayView1D<T, Stride1D.Dense> ConvertToView<T>(T[] meshes) where T : unmanaged
        {
            var buffer = GpuKernel.Accelerator.Allocate1D<T>(meshes.Length);
            buffer.CopyFromCPU(meshes);
            return buffer.View;
        }

        private MaterialModel CreateMaterialData(Scene scene)
        {
            var materials = scene.objects.Select(obj => obj.Material).ToArray();
            var diffuse = materials.Where(m => m is DiffuseMaterial).Cast<DiffuseMaterial>().ToArray();
            var fresnel = materials.Where(m => m is FresnelMaterial).Cast<FresnelMaterial>().ToArray();
            var reflection = materials.Where(m => m is ReflectionMaterial).Cast<ReflectionMaterial>().ToArray();
            var transmission = materials.Where(m => m is TransmissionMaterial).Cast<TransmissionMaterial>().ToArray();
            return new MaterialModel(diffuse, fresnel, reflection, transmission);
        }
    }
}
