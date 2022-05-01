namespace CowEngine.ImageWorker.Png
{
    using System;
    using System.Linq;
    using System.Text;

    internal static class Extensions
    {
        public static int GetBytesPerPixel(this ColorType colorType)
        {
            return
                colorType == ColorType.TruecolorAlpha ? 4 :
                colorType == ColorType.Truecolor ? 3 :
                colorType == ColorType.GreyscaleAlpha ? 2 : 1;
        }

        public static bool HasAlpha(this ColorType colorType)
        {
            return colorType == ColorType.TruecolorAlpha || colorType == ColorType.GreyscaleAlpha;
        }

        public static byte[] ToBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static byte[] FromHexString(this string s)
        {
            return Enumerable.Range(0, s.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                .ToArray();
        }

        public static int ExtractInt(this byte[] bytes, int index)
        {
            return (bytes[index] << 24) | (bytes[index + 1] << 16) | (bytes[index + 2] << 8) | bytes[index + 3];
        }

        public static byte[] ToBytes(this int i)
        {
            var bytes = BitConverter.GetBytes(i);
            return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
        }
    }
}
