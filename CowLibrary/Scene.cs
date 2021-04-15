namespace CowLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using Lights;

    public class Scene
    {
        public Camera camera = new PerspectiveCamera()
        {
            width = 1920/8,
            height = 1080/8,
            fov = 60,
        };
        
        public readonly List<Light> lights = new List<Light>();
        
        public readonly List<RenderableObject> objects = new List<RenderableObject>();

        public Box boundingBox;
        
        public void PrepareScene()
        {
            camera.transform.localToWorldMatrix = Matrix4x4Extensions.LookAt(new Vector3(0, 0, 1.5f), new Vector3(0, 0, 0), Vector3.UnitY);
            camera.transform.rotation = Quaternion.CreateFromYawPitchRoll((float) Math.PI, -90 * MathConstants.Deg2Rad,0);
            var m = camera.transform.worldToLocalMatrix;
            camera.transform.position = new Vector3(0, 0, 0);
            foreach (var obj in objects)
            {
                obj.Apply(m);
            }
            foreach (var light in lights)
            {
                light.Apply(m);
            }
            boundingBox = GetBoundingBoxFor(objects);
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
    }
}