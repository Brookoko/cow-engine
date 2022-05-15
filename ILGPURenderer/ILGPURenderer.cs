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
            var ray = rays[w / 2, h / 2, 0];
            Console.WriteLine($"{ray.origin} -> {ray.direction}");
            var hits = HitGenerator.GenerateHits(sceneData, rays);
            foreach (var hit in hits)
            {
                if (hit.HasHit)
                {
                    Console.WriteLine($"E");
                }
            }
            var colors = ColorGenerator.GenerateColors(hits);
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
            var triangles = LoadTriangles(meshes);
            var triangleMeshes = LoadTriangleMeshes(meshes);
            // var triangleMeshes = LoadMesh<TriangleMesh>(meshes);
            // var optimizedMeshes = LoadMesh<OptimizedMesh>(meshes);
            return new MeshModel(boxes, disks, planes, spheres, triangleObjects, triangles, triangleMeshes);
        }

        private ArrayView1D<T, Stride1D.Dense> LoadMesh<T>(IMesh[] meshes) where T : unmanaged, IMesh
        {
            var foundMeshes = FindMeshes<T>(meshes);
            return ConvertToView(foundMeshes);
        }

        private ArrayView1D<Triangle, Stride1D.Dense> LoadTriangles(IMesh[] meshes)
        {
            var triangleMeshes = FindMeshes<TriangleMesh>(meshes);
            var optimizedMeshes = FindMeshes<OptimizedMesh>(meshes);
            var triangles = new List<Triangle>();
            foreach (var triangleMesh in triangleMeshes)
            {
                triangles.AddRange(triangleMesh.triangles);
            }
            foreach (var triangleMesh in optimizedMeshes)
            {
                triangles.AddRange(triangleMesh.mesh.triangles);
            }
            return ConvertToView(triangles.ToArray());
        }

        private ArrayView1D<TriangleMeshModel, Stride1D.Dense> LoadTriangleMeshes(IMesh[] meshes)
        {
            var triangleMeshes = FindMeshes<TriangleMesh>(meshes);
            var triangleMeshModels = new TriangleMeshModel[triangleMeshes.Length];
            var offset = 0;
            for (var i = 0; i < triangleMeshes.Length; i++)
            {
                var triangleMesh = triangleMeshes[i];
                var count = triangleMesh.triangles.Length;
                var model = new TriangleMeshModel(offset, count);
                offset += count;
                triangleMeshModels[i] = model;
            }
            return ConvertToView(triangleMeshModels);
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
