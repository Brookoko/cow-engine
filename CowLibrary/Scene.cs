namespace CowLibrary
{
    using System.Collections.Generic;

    public class Scene
    {
        public readonly Camera camera = new Camera();
        public readonly List<RenderableObject> objects = new List<RenderableObject>();
    }
}