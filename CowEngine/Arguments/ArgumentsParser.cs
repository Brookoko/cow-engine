namespace CowEngine
{
    using System.Linq;
    using CowLibrary;

    public interface IArgumentsParser
    {
        (string source, string output) Parse(string[] args);
    }
    
    public class ArgumentsParser : IArgumentsParser
    {
        public (string source, string output) Parse(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentParseException("Insufficient number of parameters");
            }
            var source = ParseArgument("--source", args);
            var extension = source.GetExtension();
            if (extension != "obj")
            {
                throw new ArgumentParseException("Wrong source format. Obj file only supported");
            }
            var output = ParseArgument("--output", args);
            return (source, output);
        }
        
        private string ParseArgument(string parameter, string[] args)
        {
            var arg = args.FirstOrDefault(a => a.Contains(parameter));
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentParseException($"No parameter with name {parameter}");
            }
            var split = arg.Split('=');
            if (split.Length != 2 || split[0] != parameter)
            {
                throw new ArgumentParseException($"Invalid value for {parameter}");
            }
            return split[1];
        }
    }
}