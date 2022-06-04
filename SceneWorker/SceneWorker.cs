namespace SceneWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using CowEngine;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;
    using CowLibrary.Mathematics.Sampler;
    using CowRenderer;
    using Google.Protobuf.Collections;
    using SceneFormat;
    using BlendMaterial = CowLibrary.BlendMaterial;
    using Camera = CowLibrary.Camera;
    using Color = CowLibrary.Color;
    using Disk = CowLibrary.Disk;
    using FresnelMaterial = CowLibrary.FresnelMaterial;
    using Light = CowLibrary.Lights.Light;
    using MetalMaterial = CowLibrary.MetalMaterial;
    using MicrofacetReflectionMaterial = CowLibrary.MicrofacetReflectionMaterial;
    using OrenNayarMaterial = CowLibrary.OrenNayarMaterial;
    using OrthographicCamera = CowLibrary.OrthographicCamera;
    using PerspectiveCamera = CowLibrary.PerspectiveCamera;
    using Plane = CowLibrary.Plane;
    using PlasticMaterial = CowLibrary.PlasticMaterial;
    using RealisticCamera = CowLibrary.RealisticCamera;
    using Scene = CowRenderer.Scene;
    using Sphere = CowLibrary.Sphere;
    using Transform = CowLibrary.Transform;
    using Vector3 = System.Numerics.Vector3;

    public interface ISceneWorker
    {
        Scene Parse(string path);
    }

    public class SceneWorker : ISceneWorker
    {
        [Inject]
        public SceneFormat.ISceneIO SceneIo { get; set; }

        [Inject]
        public IObjWorker ObjWorker { get; set; }

        [Inject]
        public ISamplerProvider SamplerProvider { get; set; }

        [Inject]
        public RenderConfig RenderConfig { get; set; }

        public Scene Parse(string path)
        {
            var parsedScene = SceneIo.Read(path);
            return ConvertToScene(parsedScene);
        }

        private Scene ConvertToScene(SceneFormat.Scene parsedScene)
        {
            var scene = new ComplexScene();
            ParseRenderOptions(parsedScene.RenderOptions);
            scene.cameras.AddRange(parsedScene.Cameras.Select(ConvertToCamera));
            scene.SetMainCamera(parsedScene.RenderOptions.CameraId);
            var materials = ParseMaterials(parsedScene.Materials);
            scene.objects.AddRange(
                parsedScene.SceneObjects.Select((obj, index) => ConvertToObject(obj, materials, index)));
            scene.lights.AddRange(parsedScene.Lights.Select(ConvertToLight));
            return scene;
        }

        private void ParseRenderOptions(RenderOptions renderOptions)
        {
            RenderConfig.width = renderOptions.Width;
            RenderConfig.height = renderOptions.Height;
            RenderConfig.rayDepth = renderOptions.RayDepth;
            RenderConfig.numberOfRayPerPixelDimension = renderOptions.RaysPerPixelDimension;
        }

        private Camera ConvertToCamera(SceneFormat.Camera parsedCamera)
        {
            var tran = ConvertTransform(parsedCamera.Transform);
            switch (parsedCamera.CameraCase)
            {
                case SceneFormat.Camera.CameraOneofCase.Perspective:
                    return new PerspectiveCamera(RenderConfig.width, RenderConfig.height, SamplerProvider.Sampler,
                        (float)parsedCamera.Perspective.Fov)
                    {
                        Id = parsedCamera.Id,
                        Transform = tran,
                    };
                case SceneFormat.Camera.CameraOneofCase.Orthographic:
                    return new OrthographicCamera(RenderConfig.width, RenderConfig.height, SamplerProvider.Sampler)
                    {
                        Id = parsedCamera.Id,
                        Transform = tran,
                    };
                case SceneFormat.Camera.CameraOneofCase.Realistic:
                    var lens = new Lens(1, (float)parsedCamera.Realistic.Radius, (float)parsedCamera.Realistic.Focus);
                    return new RealisticCamera(RenderConfig.width, RenderConfig.height, SamplerProvider.Sampler,
                        (float)parsedCamera.Realistic.Fov, lens)
                    {
                        Id = parsedCamera.Id,
                        Transform = tran,
                    };
                default:
                    throw new Exception("Unsupported camera");
            }
        }

        private RenderableObject ConvertToObject(SceneFormat.SceneObject parsedObject,
            Dictionary<string, IMaterial> materials, int id)
        {
            var tran = ConvertTransform(parsedObject.Transform);
            var mesh = GetMesh(parsedObject, id);
            var material = GetMaterial(parsedObject, materials, id);
            return new RenderableObject(mesh, material)
            {
                Id = parsedObject.Id,
                Transform = tran,
            };
        }

        private IMesh GetMesh(SceneFormat.SceneObject parsedObject, int id)
        {
            switch (parsedObject.MeshCase)
            {
                case SceneFormat.SceneObject.MeshOneofCase.Sphere:
                    return new Sphere((float)parsedObject.Sphere.Radius, id);
                case SceneFormat.SceneObject.MeshOneofCase.Cube:
                    return new Box(ConvertVector(parsedObject.Cube.Size) * 0.5f, id);
                case SceneFormat.SceneObject.MeshOneofCase.Plane:
                    return new Plane(id);
                case SceneFormat.SceneObject.MeshOneofCase.Disk:
                    return new Disk((float)parsedObject.Disk.Radius, id);
                case SceneFormat.SceneObject.MeshOneofCase.MeshedObject:
                    return ObjWorker.Parse(parsedObject.MeshedObject.Reference, id);
                default:
                    throw new Exception("Unsupported mesh");
            }
        }

        private IMaterial GetMaterial(SceneFormat.SceneObject parsedObject, Dictionary<string, IMaterial> materials,
            int id)
        {
            switch (parsedObject.ObjectMaterialCase)
            {
                case SceneFormat.SceneObject.ObjectMaterialOneofCase.MaterialId:
                    if (materials.TryGetValue(parsedObject.MaterialId, out var material))
                    {
                        return material.Copy(id);
                    }
                    throw new Exception("Invalid material id");
                case SceneFormat.SceneObject.ObjectMaterialOneofCase.Material:
                    return ConvertMaterial(parsedObject.Material, id);
                default:
                    throw new Exception("Unsupported material");
            }
        }

        private Light ConvertToLight(SceneFormat.Light parsedLight, int id)
        {
            var tran = ConvertTransform(parsedLight.Transform);
            var color = ConvertColor(parsedLight.Color);
            switch (parsedLight.LightCase)
            {
                case SceneFormat.Light.LightOneofCase.Point:
                    return new PointLight(color, 1, id, SamplerProvider.Sampler)
                    {
                        Id = parsedLight.Id,
                        Transform = tran,
                    };
                case SceneFormat.Light.LightOneofCase.Directional:
                    return new DirectionalLight(color, 1, id, SamplerProvider.Sampler)
                    {
                        Id = parsedLight.Id,
                        Transform = tran,
                    };
                case SceneFormat.Light.LightOneofCase.Environment:
                    return new EnvironmentLight(color, 1, id, SamplerProvider.Sampler)
                    {
                        Id = parsedLight.Id,
                    };
                case SceneFormat.Light.LightOneofCase.Sphere:
                    throw new Exception("Sphere light is not supported yet");
                default:
                    throw new Exception("Unsupported light");
            }
        }

        private Dictionary<string, IMaterial> ParseMaterials(RepeatedField<SceneFormat.Material> materials)
        {
            var result = new Dictionary<string, IMaterial>();
            foreach (var material in materials)
            {
                var mat = ConvertMaterial(material, -1);
                result[material.Id] = mat;
            }
            return result;
        }

        private IMaterial ConvertMaterial(SceneFormat.Material material, int id)
        {
            switch (material.MaterialCase)
            {
                case SceneFormat.Material.MaterialOneofCase.LambertReflection:
                    return new DiffuseMaterial(ConvertColor(material.LambertReflection.Color),
                        (float)material.LambertReflection.R, id);
                case SceneFormat.Material.MaterialOneofCase.SpecularReflection:
                    return new ReflectionMaterial((float)material.SpecularReflection.R,
                        (float)material.SpecularReflection.Eta, id);
                case SceneFormat.Material.MaterialOneofCase.SpecularTransmission:
                    return new TransmissionMaterial((float)material.SpecularTransmission.T,
                        (float)material.SpecularTransmission.Eta, id);
                case SceneFormat.Material.MaterialOneofCase.Fresnel:
                    return new FresnelMaterial((float)material.Fresnel.R,
                        (float)material.Fresnel.T,
                        (float)material.Fresnel.Eta, id);
                case SceneFormat.Material.MaterialOneofCase.OrenNayar:
                    return new OrenNayarMaterial(ConvertColor(material.OrenNayar.Color),
                        (float)material.OrenNayar.R,
                        (float)material.OrenNayar.Roughness, id);
                case SceneFormat.Material.MaterialOneofCase.MicrofacetReflection:
                    return new MicrofacetReflectionMaterial(ConvertColor(material.MicrofacetReflection.Color),
                        (float)material.MicrofacetReflection.R,
                        (float)material.MicrofacetReflection.Eta,
                        (float)material.MicrofacetReflection.Roughness, id);
                case SceneFormat.Material.MaterialOneofCase.Metal:
                    return new MetalMaterial(ConvertColor(material.Metal.Color),
                        (float)material.Metal.R,
                        (float)material.Metal.Eta,
                        (float)material.Metal.K,
                        (float)material.Metal.Roughness, id);
                case SceneFormat.Material.MaterialOneofCase.Plastic:
                    return new PlasticMaterial(ConvertColor(material.Plastic.Color),
                        (float)material.Plastic.R,
                        (float)material.Plastic.Roughness, id);
                case SceneFormat.Material.MaterialOneofCase.Blend:
                    return new BlendMaterial(ConvertColor(material.Blend.Diffuse),
                        ConvertColor(material.Blend.Specular),
                        (float)material.Blend.Roughness, id);
                default:
                    throw new Exception("Unsupported material");
            }
        }

        private Transform ConvertTransform(SceneFormat.Transform t)
        {
            if (t == null) return new Transform();
            return new Transform()
            {
                LocalPosition = ConvertVector(t.Position),
                LocalRotation = ConvertToQuaternion(t.Rotation),
                LocalScale = ConvertVector(t.Scale),
            };
        }

        private Vector3 ConvertVector(SceneFormat.Vector3 v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }

        private Quaternion ConvertToQuaternion(SceneFormat.Vector3 v)
        {
            return Quaternion.CreateFromYawPitchRoll((float)(v.Y * Const.Deg2Rad), (float)(v.X * Const.Deg2Rad),
                (float)(v.Z * Const.Deg2Rad));
        }

        private Color ConvertColor(SceneFormat.Color c)
        {
            return new Color((float)c.R, (float)c.G, (float)c.B);
        }
    }
}
