namespace CowEngine
{
    using System;
    using CommandLine;

    public interface IArgumentsParser
    {
        void Parse(string[] args, Func<CompiledOptions, int> compileFlow, Func<ModelOptions, int> modelFlow);
    }
    
    public class ArgumentsParser : IArgumentsParser
    {
        public void Parse(string[] args, Func<CompiledOptions, int> compileFlow, Func<ModelOptions, int> modelFlow)
        {
            Parser.Default.ParseArguments<CompiledOptions, ModelOptions>(args)
                .MapResult(
                    compileFlow,
                    modelFlow,
                    _ => 1
                );
        }
    }
}