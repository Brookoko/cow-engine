namespace CowEngine
{
    using Cowject;
    using CowLibrary;
    using CowLibrary.Mathematics.Sampler;

    public interface IRenderableObjectWorker
    {
        RenderableObject Parse(string source);
    }

    public class RenderableObjectWorker : IRenderableObjectWorker
    {
        [Inject]
        public IObjWorker ObjWorker { get; set; }

        [Inject]
        public ISamplerProvider SamplerProvider { get; set; }
        
        public RenderableObject Parse(string source)
        {
            var mesh = ObjWorker.Parse(source);
            var material = new DiffuseMaterial(new Color(127, 0, 255), 1f, SamplerProvider.GetSampler());
            return new RenderableObject(mesh, material);
        }
    }
}
