namespace CowEngine.ImageWorker
{
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public class Deflate
    {
        private static byte[] Flags => new byte[] { 0x78, 0x9C };

        public byte[] Encode(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var res = new MemoryStream())
                {
                    using (var deflate = new DeflateStream(res, CompressionMode.Compress))
                    {
                        ms.CopyTo(deflate);
                        deflate.Close();
                        return Flags.Concat(res.ToArray()).ToArray();
                    }
                }
            }
        }
    }
}
