namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Numerics;

    public class Scene
    {
        public Camera camera = new PerspectiveCamera()
        {
            xResolution = 512,
            yReslution = 256,
            HorizontalFov = 90,
            transform = new Transform(){ position = new Vector3(0,0,-1)},
        };
        
        public readonly List<RenderableObject> objects = new List<RenderableObject>();
    }
}