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
        [Inject(Name = Options.Model)]
        public IFlow ModelFlow { get; set; }

        [Inject(Name = Options.Compiled)]
        public IFlow CompiledFlow { get; set; }

        [Inject(Name = Options.Scene)]
        public IFlow SceneFlow { get; set; }

        public void Parse(string[] args)
        {
            Parser.Default.ParseArguments<ModelOptions, CompiledOptions, SceneOptions>(args)
                .MapResult<ModelOptions, CompiledOptions, SceneOptions, int>(
                    opts => ModelFlow.Process(opts.Source, opts.Output),
                    opts => CompiledFlow.Process("", opts.Output),
                    opts => SceneFlow.Process(opts.Source, opts.Output),
                    _ => 1
                );
        }
    }
}
