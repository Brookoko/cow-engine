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
        public IFlow[] Flows { get; set; }

        public void Parse(string[] args)
        {
            Parser.Default
                .ParseArguments<Option>(args)
                .MapResult(
                    Process,
                    _ => 1
                );
        }

        private int Process(Option option)
        {
            foreach (var flow in Flows)
            {
                if (flow.CanWorkWithProcess(option))
                {
                    return flow.Process(option);
                }
            }
            return 1;
        }
    }
}
