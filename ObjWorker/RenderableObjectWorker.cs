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
            var material = new DiffuseMaterial(new Color(127, 0, 255), 1f);
            return new RenderableObject(mesh, material);
        }
    }
}