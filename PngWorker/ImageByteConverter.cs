namespace CowEngine.ImageWorker.Png
{
    using CowLibrary;

    internal interface IImageByteConverter
    {
        byte[,] ToBytes(Image image);
    }

    internal class ImageByteConverter : IImageByteConverter
    {
        public byte[,] ToBytes(Image image)
        {
            var h = image.Height;
            var w = image.Width;
            var bytes = new byte[h, 3 * w];
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < 3 * w; j += 3)
                {
                    var b = image[i, j / 3].ToBytes();
                    bytes[i, j] = b[0];
                    bytes[i, j + 1] = b[1];
                    bytes[i, j + 2] = b[2];
                }
            }
            return bytes;
        }
    }
}
