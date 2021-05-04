namespace CowEngine
{
    using System;
    using CommandLine;

    public interface IArgumentsParser
    {
        void Parse(string[] args, Func<CompiledOptions, int> compileFlow, Func<ModelOptions, int> modelFlow, Func<SceneOptions, int> sceneFlow);
    }
    
    public class ArgumentsParser : IArgumentsParser
    {
        public void Parse(string[] args, Func<CompiledOptions, int> compileFlow, Func<ModelOptions, int> modelFlow, Func<SceneOptions, int> sceneFlow)
        {
            Parser.Default.ParseArguments<CompiledOptions, ModelOptions, SceneOptions>(args)
                .MapResult(
                    compileFlow,
                    modelFlow,
                    sceneFlow,
                    _ => 1
                );
        }
    }
}