namespace CowEngine
{
    using Cowject;
    using CowLibrary;

    public interface IRenderableObjectWorker
    {
        RenderableObject Parse(string source, int id);
    }

    public class RenderableObjectWorker : IRenderableObjectWorker
    {
        [Inject]
        public IObjWorker ObjWorker { get; set; }

        public RenderableObject Parse(string source, int id)
        {
            var mesh = ObjWorker.Parse(source, id);
            var material = new DiffuseMaterial(new Color(127, 0, 255), 1f, id);
            return new RenderableObject(mesh, material);
        }
    }
}
