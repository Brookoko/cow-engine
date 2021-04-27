namespace CowEngine
{
    using CommandLine;

    [Verb("compiled", HelpText = "Render with compiled scene")]
    public class CompiledOptions
    {
        [Option("output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }
    
    [Verb("model", HelpText = "Render with compiled scene")]
    public class ModelOptions
    {
        [Option("source", Required = true, HelpText = "Source model")]
        public string Source { get; set; }
        
        [Option("output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }
}