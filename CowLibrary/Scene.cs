namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Numerics;

    public class Scene
    {
        public Camera camera = new PerspectiveCamera()
        {
            width = 512,
            height = 512,
            fov = 60,
        };
        
        public readonly List<RenderableObject> objects = new List<RenderableObject>();
        
        public void PrepareScene()
        {
            // camera.transform.position = new Vector3(0, 0, 3);
            // camera.transform.rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 30 * MathConstants.Deg2Rad);
            camera.transform.localToWorldMatrix = Matrix4x4Extensions.LookAt(new Vector3(0, 0, 3), new Vector3(2, 0, 0), Vector3.UnitY);
            var m = camera.transform.worldToLocalMatrix;
            foreach (var obj in objects)
            {
                obj.Prepare(m);
            }
        }
    }
}