namespace CowEngine.ImageWorker
{
    using System.IO;

    public class FilesBypassingWriter : IBytesWriter
    {
        private readonly string bypassFormat;

        public FilesBypassingWriter(string bypassDelimiter)
        {
            bypassFormat = bypassDelimiter + "{0}";
        }
        
        public void Write(byte[] bytes, string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            if (File.Exists(path))
            {
                path = BypassToNonExistingFile(path);
            }
            
            var fileInfo = new FileInfo(path);
            fileInfo.Directory?.Create();
            File.WriteAllBytes(path, bytes);
        }

        private string BypassToNonExistingFile(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var fileNameNoExt = Path.GetFileNameWithoutExtension(path);
            var fileExtension = Path.GetExtension(path);
            var bypassCounter = 0;
            
            while (File.Exists(path))
            {
                var bypassFileName = fileNameNoExt + string.Format(bypassFormat, ++bypassCounter) + fileExtension;
                path = Path.Combine(directory, bypassFileName);
            }

            return path;
        }
    }
}