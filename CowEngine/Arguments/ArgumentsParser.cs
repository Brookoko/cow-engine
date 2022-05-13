namespace CowEngine
{
    using CommandLine;
    using Cowject;

    public interface IArgumentsParser
    {
        void Parse(string[] args);
    }

    public class ArgumentsParser : IArgumentsParser
    {
        [Inject]
        public IFlow<CpuOption> CpuFlow { get; set; }

        [Inject]
        public IFlow<GpuOption> GpuFlow { get; set; }

        public void Parse(string[] args)
        {
            Parser.Default
                .ParseArguments<CpuOption, GpuOption>(args)
                .MapResult<CpuOption, GpuOption, int>(
                    CpuFlow.Process,
                    GpuFlow.Process,
                    _ => 1
                );
        }
    }
}
