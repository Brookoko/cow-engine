namespace CowEngine.ImageWorker.Png
{
    using System.Linq;
    using CowEngine.ImageWorker;
    using CowLibrary;

    public class PngWorker : IImageEncoder
    {
        private static string pngFormatHeader = "89504E470D0A1A0A";

        private readonly Deflate deflate = new Deflate();
        private readonly IByteFilterer byteFilterer = new ByteFilterer();
        private readonly IImageByteConverter imageByteConverter = new ImageByteConverter();

        public bool CanWorkWith(string extension)
        {
            return extension == "png";
        }

        public byte[] Encode(in Image image)
        {
            var header = CreateHeader(in image);
            var data = imageByteConverter.ToBytes(in image);
            var filtered = byteFilterer.Filter(data, FilterType.None);
            var encoded = deflate.Encode(filtered);

            var chunks = new[] { header.ToChunk(), CreateDataChunk(encoded), CreateEndChunk() };
            var content = chunks.SelectMany(c => c.ToBytes());
            var pngHeader = pngFormatHeader.FromHexString();

            return pngHeader.Concat(content).ToArray();
        }

        private Header CreateHeader(in Image image)
        {
            return new Header()
            {
                bitDepth = 8,
                colorType = ColorType.Truecolor,
                compression = 0,
                filterMethod = 0,
                height = image.Height,
                width = image.Width,
                transferMethod = 0
            };
        }

        private Chunk CreateDataChunk(byte[] data)
        {
            return new Chunk(data, ChunkType.Data);
        }

        private Chunk CreateEndChunk()
        {
            return new Chunk(new byte[0], ChunkType.End);
        }
    }
}
