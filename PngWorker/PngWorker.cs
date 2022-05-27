namespace CowEngine.ImageWorker.Png
{
    using System.Linq;
    using CowEngine.ImageWorker;
    using Cowject;
    using CowLibrary;

    public class PngWorker : IImageEncoder
    {
        private const string PngFormatHeader = "89504E470D0A1A0A";

        [Inject]
        public Deflate Deflate { get; set; }

        [Inject]
        internal IByteFilterer ByteFilterer { get; set; }
        
        [Inject]
        internal IImageByteConverter ImageByteConverter { get; set; }
        
        public bool CanWorkWith(string extension)
        {
            return extension == "png";
        }

        public byte[] Encode(in Image image)
        {
            var header = CreateHeader(in image);
            var data = ImageByteConverter.ToBytes(in image);
            var filtered = ByteFilterer.Filter(data, FilterType.None);
            var encoded = Deflate.Encode(filtered);

            var chunks = new[] { header.ToChunk(), CreateDataChunk(encoded), CreateEndChunk() };
            var content = chunks.SelectMany(c => c.ToBytes());
            var pngHeader = PngFormatHeader.FromHexString();

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
