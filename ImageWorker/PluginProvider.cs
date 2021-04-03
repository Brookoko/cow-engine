namespace CowEngine.ImageWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal interface IPluginProvider
    {
        IEnumerable<Type> Types { get; }
    }
    
    public class PluginProvider : IPluginProvider
    {
        public IEnumerable<Type> Types => AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(ass => ass.GetExportedTypes())
            .ToList();
    }
}