namespace CowEngine.ImageWorker
{
    using Cowject;

    public interface IBytesReader
    {
        byte[] Read(string path);
    }

    public interface IBytesWriter
    {
        void Write(byte[] bytes, string path);
    }

    public interface IIoWorker : IBytesReader, IBytesWriter
    {
    }

    public class IoWorker : IIoWorker
    {
        [Inject]
        public IBytesReader BytesReader { get; set; }

        [Inject]
        public IBytesWriter BytesWriter { get; set; }

        public byte[] Read(string path) => BytesReader.Read(path);

        public void Write(byte[] bytes, string path) => BytesWriter.Write(bytes, path);
    }
}
