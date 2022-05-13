namespace CowEngine
{
    using CommandLine;

    [Verb("cpu", HelpText = "Render scene")]
    public class CpuOption : Option
    {
    }
    
    [Verb("gpu", HelpText = "Render scene")]
    public class GpuOption : Option
    {
    }
    
    public class Option
    {
        [Option('m', "model", Required = false, HelpText = "Source model", Group = "Source")]
        public string Model { get; set; }
        
        [Option('s', "source", Required = false, HelpText = "Source scene", Group = "Source")]
        public string Scene { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }

    public enum Options
    {
        Cpu,
        Gpu,
    }
}
