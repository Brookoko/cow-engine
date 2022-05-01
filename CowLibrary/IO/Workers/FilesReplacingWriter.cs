namespace CowEngine.ImageWorker
{
    using System.IO;

    public class FilesReplacingWriter : IBytesWriter
    {
        public void Write(byte[] bytes, string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            var fileInfo = new FileInfo(path);
            fileInfo.Directory?.Create();
            File.WriteAllBytes(path, bytes);
        }
    }
}
