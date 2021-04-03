namespace CowEngine
{
    using CowLibrary;

    public interface IObjWorker
    {
        RenderableObject Parse(string source);
    }
    
    public class ObjWorker : IObjWorker
    {
        public RenderableObject Parse(string source)
        {
            return null;
        }
    }
}