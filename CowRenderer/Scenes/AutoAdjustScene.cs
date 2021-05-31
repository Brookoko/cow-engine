namespace CowRenderer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using Cowject;
    using CowLibrary;

    public class AutoAdjustScene : Scene
    {
        [Inject]
        public RenderConfig RenderConfig { get; set; }

        public override Camera MainCamera => camera;

        private RealisticCamera camera;
        
        public override void PrepareScene()
        {
            camera = CreateCamera();
            cameras.Add(camera);
            var box = GetBoundingBoxFor(objects);
            PlaceCamera(box);
            PlacePlane(box);
            base.PrepareScene();
        }
        
        private RealisticCamera CreateCamera()
        {
            return new RealisticCamera(RenderConfig.width, RenderConfig.height, RenderConfig.fov, new Lens(1f, 0.01f, 1f));
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
            
            camera.transform.localToWorldMatrix = Matrix4x4Extensions.LookAt(new Vector3(0, 0.5f, dist), box.center);
        }
        
        private void PlacePlane(Box box)
        {
            var plane = new RenderableObject(new Disk(100), new DiffuseMaterial(Color.Red, 1));
            plane.transform.position = box.min.Y * Vector3.UnitY;
            objects.Add(plane);
        }
    }
}