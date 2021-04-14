namespace CowLibrary
{
    using System.Collections.Generic;
    using System.Numerics;

    public class Scene
    {
        public readonly Camera camera = new PerspectiveCamera()
        {
            xResolution = 1920,
            yReslution = 1080,
            HorizontalFov = 75,
            transform = new Transform(){ position = new Vector3(0,0,-4)},
        };
        
        public readonly List<RenderableObject> objects = new List<RenderableObject>();
    }
}