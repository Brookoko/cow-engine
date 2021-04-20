namespace CowRenderer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;

    public class Scene
    {
        [Inject]
        public RenderConfig RenderConfig { get; set; }
        
        public PerspectiveCamera camera;
        
        public readonly List<Light> lights = new List<Light>();
        
        public readonly List<RenderableObject> objects = new List<RenderableObject>();

        public void PrepareScene()
        {
            camera = CreateCamera();
            var box = GetBoundingBoxFor(objects);
            PlaceCamera(box);
            foreach (var obj in objects)
            {
                obj.Prepare();
            }
        }
        
        private PerspectiveCamera CreateCamera()
        {
            return new PerspectiveCamera()
            {
                width = RenderConfig.width,
                height = RenderConfig.height,
                fov = RenderConfig.fov
            };
        }
        
        private Box GetBoundingBoxFor(List<RenderableObject> renderableObjects)
        {
            var min = renderableObjects.First().mesh.BoundingBox.min;
            var max = renderableObjects.First().mesh.BoundingBox.max;
            foreach (var renderableObject in renderableObjects)
            {
                var objectBoundingBox = renderableObject.mesh.BoundingBox;
                min.X = Math.Min(min.X, objectBoundingBox.min.X);
                min.Y = Math.Min(min.Y, objectBoundingBox.min.Y);
                min.Z = Math.Min(min.Z, objectBoundingBox.min.Z);
                max.X = Math.Max(max.X, objectBoundingBox.max.X);
                max.Y = Math.Max(max.Y, objectBoundingBox.max.Y);
                max.Z = Math.Max(max.Z, objectBoundingBox.max.Z);
            }

            return new Box(min, max);
        }
        
        private void PlaceCamera(Box box)
        {
            var max = Math.Max(box.size.X * 1.3f, box.size.Y * 1.3f * camera.aspectRatio);
            var tan = (float) Math.Tan(Const.Deg2Rad * camera.fov / 2);
            var dist = max / tan;
            
            camera.transform.localToWorldMatrix = Matrix4x4Extensions.LookAt(new Vector3(0, 0, dist), box.center);
        }
    }
}