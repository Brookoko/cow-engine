using System.IO;

namespace ObjLoader.Loader.Loaders
{
    using System.Text.RegularExpressions;

    public abstract class LoaderBase
    {
        private StreamReader _lineStreamReader;

        protected void StartLoad(Stream lineStream)
        {
            _lineStreamReader = new StreamReader(lineStream);

            while (!_lineStreamReader.EndOfStream)
            {
                ParseLine();
            }
        }

        private void ParseLine()
        {
            var currentLine = _lineStreamReader.ReadLine();

            if (string.IsNullOrWhiteSpace(currentLine) || currentLine[0] == '#')
            {
                return;
            }

            var fields = currentLine.Trim().Split(null, 2);
            var keyword = fields[0].Trim();
            var data = Regex.Replace(fields[1].Trim(), @"\s+", " ");

            ParseLine(keyword, data);
        }

        protected abstract void ParseLine(string keyword, string data);
    }
}