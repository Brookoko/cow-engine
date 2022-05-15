namespace CowRenderer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Mathematics.Sampler;

    public class AutoAdjustScene : Scene
    {
        [Inject]
        public RenderConfig RenderConfig { get; set; }

        public override Camera MainCamera => camera;

        private readonly ISampler sampler;
        private RealisticCamera camera;

        public AutoAdjustScene(ISampler sampler)
        {
            this.sampler = sampler;
        }

        public override void PrepareScene()
        {
            camera = CreateCamera();
            cameras.Add(camera);
            var bound = GetBoundingBoxFor(objects);
            PlaceCamera(bound);
            PlacePlane(bound);
            base.PrepareScene();
        }

        private RealisticCamera CreateCamera()
        {
            return new RealisticCamera(RenderConfig.width, RenderConfig.height, sampler, RenderConfig.fov,
                new Lens(1f, 0.01f, 1f));
        }

        private Bound GetBoundingBoxFor(List<RenderableObject> renderableObjects)
        {
            var min = renderableObjects.First().Mesh.GetBoundingBox().min;
            var max = renderableObjects.First().Mesh.GetBoundingBox().max;
            foreach (var renderableObject in renderableObjects)
            {
                var objectBoundingBox = renderableObject.Mesh.GetBoundingBox();
                min.X = Math.Min(min.X, objectBoundingBox.min.X);
                min.Y = Math.Min(min.Y, objectBoundingBox.min.Y);
                min.Z = Math.Min(min.Z, objectBoundingBox.min.Z);
                max.X = Math.Max(max.X, objectBoundingBox.max.X);
                max.Y = Math.Max(max.Y, objectBoundingBox.max.Y);
                max.Z = Math.Max(max.Z, objectBoundingBox.max.Z);
            }

            return new Bound(min, max);
        }

        private void PlaceCamera(Bound box)
        {
            var max = Math.Max(box.size.X * 1.3f, box.size.Y * 1.3f * camera.AspectRatio);
            var tan = (float)Math.Tan(Const.Deg2Rad * camera.Fov / 2);
            var dist = max / tan;

            camera.Transform.LocalToWorldMatrix = Matrix4x4Extensions.LookAt(new Vector3(0, 0.5f, dist), box.center);
        }

        private void PlacePlane(Bound box)
        {
            var plane = new RenderableObject(new Disk(100), new DiffuseMaterial(Color.Red, 1));
            plane.Transform.Position = box.min.Y * Vector3.UnitY;
            objects.Add(plane);
        }
    }
}
