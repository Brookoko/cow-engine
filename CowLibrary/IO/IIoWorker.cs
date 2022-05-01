namespace CowEngine.ImageWorker
{
    public interface IIoWorker : IBytesReader, IBytesWriter
    {
    }

    public interface IBytesReader
    {
        byte[] Read(string path);
    }

    public interface IBytesWriter
    {
        void Write(byte[] bytes, string path);
    }
}
