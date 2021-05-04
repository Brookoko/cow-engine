namespace SceneWorker
{
    using System;
    using System.Linq;
    using System.Numerics;
    using CowEngine;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;
    using CowRenderer;
    using SceneFormat;
    using Camera = CowLibrary.Camera;
    using OrthographicCamera = CowLibrary.OrthographicCamera;
    using PerspectiveCamera = CowLibrary.PerspectiveCamera;
    using Scene = CowRenderer.Scene;
    using Sphere = CowLibrary.Sphere;
    using Material = CowLibrary.Material;
    using Light = CowLibrary.Lights.Light;
    using Color = CowLibrary.Color;
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
            scene.cameras.AddRange(parsedScene.Cameras.Select(ConvertToCamera));
            scene.SetMainCamera(parsedScene.RenderOptions.CameraId);
            RenderConfig.width = parsedScene.RenderOptions.Width;
            RenderConfig.height = parsedScene.RenderOptions.Height;
            scene.objects.AddRange(parsedScene.SceneObjects.Select(ConvertToObject));
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
                        id = parsedCamera.Id,
                        transform = tran,
                        width = RenderConfig.width,
                        height = RenderConfig.height,
                        fov = (float) parsedCamera.Perspective.Fov,
                    };
                case SceneFormat.Camera.CameraOneofCase.Orthographic:
                    return new OrthographicCamera()
                    {
                        id = parsedCamera.Id,
                        transform = tran,
                        width = RenderConfig.width,
                        height = RenderConfig.height,
                    };
                default:
                    throw new Exception("Unsupported camera");
            }
        }
        
        private RenderableObject ConvertToObject(SceneFormat.SceneObject parsedObject)
        {
            var tran = ConvertTransform(parsedObject.Transform);
            var mesh = GetMesh(parsedObject);
            var material = new Material(){color = new Color(1f)};
            return new RenderableObject()
            {
                id = parsedObject.Id,
                transform = tran,
                mesh = mesh,
                material = material,
            };
        }
        
        private Mesh GetMesh(SceneFormat.SceneObject parsedObject)
        {
            switch (parsedObject.MeshCase)
            {
                case SceneFormat.SceneObject.MeshOneofCase.Sphere:
                    return new Sphere(1);
                case SceneFormat.SceneObject.MeshOneofCase.Cube:
                    return new Box(Vector3.Zero, 1);
                case SceneFormat.SceneObject.MeshOneofCase.Plane:
                    throw new Exception("Plane is not supported yet");
                case SceneFormat.SceneObject.MeshOneofCase.MeshedObject:
                    return ObjWorker.Parse(parsedObject.MeshedObject.Reference);
                default:
                    throw new Exception("Unsupported mesh");
            }
        }

        private Light ConvertToLight(SceneFormat.Light parsedLight)
        {
            var tran = ConvertTransform(parsedLight.Transform);
            var intensity = (float) parsedLight.Intensity;
            var color = CovertColor(parsedLight.Color);
            switch (parsedLight.LightCase)
            {
                case SceneFormat.Light.LightOneofCase.Point:
                    return new PointLight(color, intensity)
                    {
                        id = parsedLight.Id,
                        transform = tran,
                    };
                case SceneFormat.Light.LightOneofCase.Directional:
                    return new DirectionalLight(color, intensity)
                    {
                        id = parsedLight.Id,
                        transform = tran,
                    };
                case SceneFormat.Light.LightOneofCase.Sphere:
                    throw new Exception("Sphere light is not supported yet");
                default:
                    throw new Exception("Unsupported light");
            }
        }
        
        private Transform ConvertTransform(SceneFormat.Transform t)
        {
            if (t == null) return new Transform();
            return new Transform()
            {
                localPosition = ConvertVector(t.Position),
                localRotation = ConvertToQuaternion(t.Rotation),
                localScale = ConvertVector(t.Scale, Vector3.One),
            };
        }
        
        private Vector3 ConvertVector(SceneFormat.Vector3 v)
        {
            return ConvertVector(v, Vector3.Zero);
        }
        
        private Vector3 ConvertVector(SceneFormat.Vector3 v, Vector3 def)
        {
            return v == null ? def : new Vector3((float) v.X, (float) v.Y, (float) v.Z);
        }
        
        private Quaternion ConvertToQuaternion(SceneFormat.Vector3 v)
        {
            return v == null ? Quaternion.Identity :
                Quaternion.CreateFromYawPitchRoll((float) (v.Y * Const.Deg2Rad), (float) (v.X * Const.Deg2Rad), (float) (v.Z * Const.Deg2Rad));
        }
        
        private Color CovertColor(SceneFormat.Color c)
        {
            return c == null ? new Color(1f) : new Color((float) c.R, (float) c.G, (float) c.B);
        }
    }
}