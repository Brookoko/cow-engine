namespace CowEngine.ImageWorker.Png
{
    internal interface IByteFilterer
    {
        byte[] Filter(byte[,] data, FilterType type);
    }

    internal class ByteFilterer : IByteFilterer
    {
        public byte[] Filter(byte[,] data, FilterType type)
        {
            var h = data.GetLength(0);
            var w = data.GetLength(1) + 1;
            var filtered = new byte[w * h];
            var function = FilterFunctionLibrary.GetFunction(type);
            var filterByte = (byte)type;

            for (var i = 0; i < h; i++)
            {
                filtered[i * w] = filterByte;
                for (var j = 0; j < w - 1; j++)
                {
                    var x = data[i, j];
                    var a = j > 0 ? data[i, j - 1] : 0;
                    var b = i > 0 ? data[i - 1, j] : 0;
                    var c = i > 0 && j > 0 ? data[i - 1, j - 1] : 0;
                    var v = function(x, a, b, c);
                    filtered[j + 1 + i * w] = (byte)((v + 256) % 256);
                }
            }
            return filtered;
        }
    }
}
