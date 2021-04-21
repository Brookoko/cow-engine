namespace CowEngine.ImageWorker
{
    using System;
    using System.IO;

    public class SimpleBytesReader : IBytesReader
    {
        public byte[] Read(string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist");
            }
            return File.ReadAllBytes(path);
        }
    }
}