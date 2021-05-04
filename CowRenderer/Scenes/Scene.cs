namespace CowRenderer
{
    using System.Collections.Generic;
    using CowLibrary;
    using CowLibrary.Lights;

    public abstract class Scene
    {
        public abstract Camera MainCamera { get; }
        
        public readonly List<Camera> cameras = new List<Camera>();
        
        public readonly List<Light> lights = new List<Light>();
        
        public readonly List<RenderableObject> objects = new List<RenderableObject>();

        public virtual void PrepareScene()
        {
            foreach (var obj in objects)
            {
                obj.Prepare();
            }
        }
    }
}