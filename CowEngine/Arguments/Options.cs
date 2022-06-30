namespace CowEngine
{
    using CommandLine;

    public class Option
    {
        [Option('m', "mode", Required = true, HelpText = "Source model")]
        public string Mode { get; set; }

        [Option("model", Required = false, HelpText = "Source model", Group = "Source")]
        public string Model { get; set; }

        [Option('s', "source", Required = false, HelpText = "Source scene", Group = "Source")]
        public string Scene { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }
}
