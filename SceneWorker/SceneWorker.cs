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
    using CowRenderer;
    using Google.Protobuf.Collections;
    using SceneFormat;
    using Camera = CowLibrary.Camera;
    using OrthographicCamera = CowLibrary.OrthographicCamera;
    using PerspectiveCamera = CowLibrary.PerspectiveCamera;
    using Scene = CowRenderer.Scene;
    using Sphere = CowLibrary.Sphere;
    using Plane = CowLibrary.Plane;
    using Disk = CowLibrary.Disk;
    using Light = CowLibrary.Lights.Light;
    using Color = CowLibrary.Color;
    using Material = CowLibrary.Material;
    using SceneObject = SceneFormat.SceneObject;
    using Transform = CowLibrary.Transform;
    using Vector3 = System.Numerics.Vector3;

    public interface ISceneWorker
    {
        Scene Parse(string path);
    }
    
    public class SceneWorker : ISceneWorker
    {
        [Inject]
        public ISceneIO SceneIo { get; set; }
        
        [Inject]
        public IObjWorker ObjWorker { get; set; }
        
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
            RenderConfig.width = parsedScene.RenderOptions.Width;
            RenderConfig.height = parsedScene.RenderOptions.Height;
            scene.cameras.AddRange(parsedScene.Cameras.Select(ConvertToCamera));
            scene.SetMainCamera(parsedScene.RenderOptions.CameraId);
            var materials = ParseMaterials(parsedScene.Materials);
            scene.objects.AddRange(parsedScene.SceneObjects.Select(obj => ConvertToObject(obj, materials)));
            scene.lights.AddRange(parsedScene.Lights.Select(ConvertToLight));
            return scene;
        }
        
        private Camera ConvertToCamera(SceneFormat.Camera parsedCamera)
        {
            var tran = ConvertTransform(parsedCamera.Transform);
            switch (parsedCamera.CameraCase)
            {
                case SceneFormat.Camera.CameraOneofCase.Perspective:
                    return new PerspectiveCamera()
                    {
                        Id = parsedCamera.Id,
                        Transform = tran,
                        width = RenderConfig.width,
                        height = RenderConfig.height,
                        Fov = (float) parsedCamera.Perspective.Fov,
                    };
                case SceneFormat.Camera.CameraOneofCase.Orthographic:
                    return new OrthographicCamera()
                    {
                        Id = parsedCamera.Id,
                        Transform = tran,
                        width = RenderConfig.width,
                        height = RenderConfig.height,
                    };
                default:
                    throw new Exception("Unsupported camera");
            }
        }
        
        private RenderableObject ConvertToObject(SceneFormat.SceneObject parsedObject, Dictionary<string, Material> materials)
        {
            var tran = ConvertTransform(parsedObject.Transform);
            var mesh = GetMesh(parsedObject);
            var material = GetMaterial(parsedObject, materials);
            return new RenderableObject(mesh, material)
            {
                Id = parsedObject.Id,
                Transform = tran,
            };
        }
        
        private Mesh GetMesh(SceneFormat.SceneObject parsedObject)
        {
            switch (parsedObject.MeshCase)
            {
                case SceneFormat.SceneObject.MeshOneofCase.Sphere:
                    return new Sphere((float) parsedObject.Sphere.Radius);
                case SceneFormat.SceneObject.MeshOneofCase.Cube:
                    return new Box(ConvertVector(parsedObject.Cube.Size) * 0.5f);
                case SceneFormat.SceneObject.MeshOneofCase.Plane:
                    return new Plane();
                case SceneFormat.SceneObject.MeshOneofCase.Disk:
                    return new Disk((float) parsedObject.Disk.Radius);
                case SceneFormat.SceneObject.MeshOneofCase.MeshedObject:
                    return ObjWorker.Parse(parsedObject.MeshedObject.Reference);
                default:
                    throw new Exception("Unsupported mesh");
            }
        }

        private Material GetMaterial(SceneObject parsedObject, Dictionary<string, Material> materials)
        {
            switch (parsedObject.ObjectMaterialCase)
            {
                case SceneObject.ObjectMaterialOneofCase.MaterialId:
                    if (materials.TryGetValue(parsedObject.MaterialId, out var material))
                    {
                        return material;
                    }
                    throw new Exception("Invalid material id");
                case SceneObject.ObjectMaterialOneofCase.Material:
                    return ConvertMaterial(parsedObject.Material);
                default:
                    throw new Exception("Unsupported material");
            }
        }
        
        private Light ConvertToLight(SceneFormat.Light parsedLight)
        {
            var tran = ConvertTransform(parsedLight.Transform);
            var color = ConvertColor(parsedLight.Color);
            switch (parsedLight.LightCase)
            {
                case SceneFormat.Light.LightOneofCase.Point:
                    return new PointLight(color, 1)
                    {
                        Id = parsedLight.Id,
                        Transform = tran,
                    };
                case SceneFormat.Light.LightOneofCase.Directional:
                    return new DirectionalLight(color, 1)
                    {
                        Id = parsedLight.Id,
                        Transform = tran,
                    };
                case SceneFormat.Light.LightOneofCase.Environment:
                    return new EnvironmentLight(color, 1)
                    {
                        Id = parsedLight.Id,
                    };
                case SceneFormat.Light.LightOneofCase.Sphere:
                    throw new Exception("Sphere light is not supported yet");
                default:
                    throw new Exception("Unsupported light");
            }
        }
        
        private Dictionary<string, Material> ParseMaterials(RepeatedField<SceneFormat.Material> materials)
        {
            var result = new Dictionary<string, Material>();
            foreach (var material in materials)
            {
                var mat = ConvertMaterial(material);
                result[material.Id] = mat;
            }
            return result;
        }
        
        private Material ConvertMaterial(SceneFormat.Material material)
        {
            switch (material.MaterialCase)
            {
                case SceneFormat.Material.MaterialOneofCase.LambertReflection:
                    return new DiffuseMaterial(ConvertColor(material.LambertReflection.Color), 1);
                case SceneFormat.Material.MaterialOneofCase.SpecularReflection:
                    return new ReflectionMaterial(1, (float) material.SpecularReflection.Eta);
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
            return new Vector3((float) v.X, (float) v.Y, (float) v.Z);
        }
        
        private Quaternion ConvertToQuaternion(SceneFormat.Vector3 v)
        {
            return Quaternion.CreateFromYawPitchRoll((float) (v.Y * Const.Deg2Rad), (float) (v.X * Const.Deg2Rad), (float) (v.Z * Const.Deg2Rad));
        }
        
        private Color ConvertColor(SceneFormat.Color c)
        {
            return new Color((float) c.R, (float) c.G, (float) c.B);
        }
    }
}