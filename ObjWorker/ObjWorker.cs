namespace CowEngine
{
    using System.IO;
    using Cowject;
    using CowLibrary;
    using ImageWorker;
    using ObjLoader.Loader.Loaders;

    public interface IObjWorker
    {
        Mesh Parse(string source);
    }

    public class ObjWorker : IObjWorker
    {
        [Inject]
        public IObjLoaderFactory ObjLoaderFactory { get; set; }

        [Inject]
        public IIoWorker IoWorker { get; set; }

        [Inject]
        public IModelToObjectConverter ModelToObjectConverter { get; set; }

        public Mesh Parse(string source)
        {
            var objLoader = ObjLoaderFactory.Create();
            var bytes = IoWorker.Read(source);
            using (var stream = new MemoryStream(bytes))
            {
                var result = objLoader.Load(stream);
                return ModelToObjectConverter.Convert(result);
            }
        }
    }
}
