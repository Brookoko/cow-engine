namespace CowLibrary
{
    using System.Linq;

    public static class StringExtensions
    {
        public static string GetExtension(this string path)
        {
            return path.Split('.').Last();
        }
    }
}