namespace CowEngine
{
    using Cowject;
    using CowLibrary;

    public interface IRenderableObjectWorker
    {
        RenderableObject Parse(string source);
    }
    
    public class RenderableObjectWorker : IRenderableObjectWorker
    {
        [Inject]
        public IObjWorker ObjWorker { get; set; }
        
        public RenderableObject Parse(string source)
        {
            var mesh = ObjWorker.Parse(source);
            var material = new Material {color = new Color(127, 0, 255)};
            return new RenderableObject(mesh, material);
        }
    }
}