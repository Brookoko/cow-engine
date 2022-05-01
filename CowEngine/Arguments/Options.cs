namespace CowEngine
{
    using CommandLine;

    [Verb("compiled", HelpText = "Render compiled scene")]
    public class CompiledOptions
    {
        [Option("output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }

    [Verb("model", HelpText = "Render model")]
    public class ModelOptions
    {
        [Option("source", Required = true, HelpText = "Source model")]
        public string Source { get; set; }

        [Option("output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }

    [Verb("scene", HelpText = "Render scene")]
    public class SceneOptions
    {
        [Option("source", Required = true, HelpText = "Source scene")]
        public string Source { get; set; }

        [Option("output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }

    public enum Options
    {
        Model,
        Compiled,
        Scene
    }
}
