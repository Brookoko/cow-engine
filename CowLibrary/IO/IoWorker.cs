namespace CowEngine.ImageWorker
{
    public class IoWorker : IIoWorker
    {
        private readonly IBytesReader bytesReader = new SimpleBytesReader();

        private readonly IBytesWriter bytesWriter = new FilesBypassingWriter("-");

        public byte[] Read(string path) => bytesReader.Read(path);

        public void Write(byte[] bytes, string path) => bytesWriter.Write(bytes, path);
    }
}